﻿namespace FoodExpress.ServiceDefaults.OpenApi;

internal static class OpenApiOptionsExtensions
{
    public static OpenApiOptions ApplyApiVersionInfo(this OpenApiOptions options,
                                                     string title,
                                                     string description)
    {
        options.AddDocumentTransformer(
            (document, context, _) =>
            {
                var versionedDescriptionProvider =
                    context.ApplicationServices.GetService<IApiVersionDescriptionProvider>();
                var apiDescription =
                    versionedDescriptionProvider?.ApiVersionDescriptions.SingleOrDefault(
                        versionDescription => versionDescription.GroupName == context.DocumentName);

                if (apiDescription is null)
                {
                    return Task.CompletedTask;
                }

                document.Info.Version = apiDescription.ApiVersion.ToString();
                document.Info.Title = title;
                document.Info.Description = BuildDescription(apiDescription, description);

                return Task.CompletedTask;
            });

        return options;
    }

    private static string BuildDescription(ApiVersionDescription api, string description)
    {
        var text = new StringBuilder(description);

        if (api.IsDeprecated)
        {
            if (text.Length > 0)
            {
                if (text[^1] != '.')
                {
                    text.Append('.');
                }

                text.Append(' ');
            }

            text.Append("This API version has been deprecated.");
        }

        if (api.SunsetPolicy is not { } policy)
            return text.ToString();

        if (policy.Date is { } when)
        {
            if (text.Length > 0)
            {
                text.Append(' ');
            }

            text.Append("The API will be sunset on ")
                .Append(when.Date.ToShortDateString())
                .Append('.');
        }

        if (!policy.HasLinks)
            return text.ToString();

        text.AppendLine();

        var rendered = false;

        foreach (var link in policy.Links.Where(l => l.Type == "text/html"))
        {
            if (!rendered)
            {
                text.Append("<h4>Links</h4><ul>");
                rendered = true;
            }

            text.Append("<li><a href=\"");
            text.Append(link.LinkTarget.OriginalString);
            text.Append("\">");
            text.Append(
                StringSegment.IsNullOrEmpty(link.Title)
                    ? link.LinkTarget.OriginalString
                    : link.Title.ToString());
            text.Append("</a></li>");
        }

        if (rendered)
        {
            text.Append("</ul>");
        }

        return text.ToString();
    }

    public static OpenApiOptions ApplySecuritySchemeDefinitions(this OpenApiOptions options)
    {
        options.AddDocumentTransformer<SecuritySchemeDefinitionsTransformer>();

        return options;
    }

    public static OpenApiOptions ApplyAuthorizationChecks(this OpenApiOptions options,
                                                          string[] scopes)
    {
        options.AddOperationTransformer(
            (operation, context, _) =>
            {
                var metadata = context.Description.ActionDescriptor.EndpointMetadata;

                if (!metadata.OfType<IAuthorizeData>().Any())
                {
                    return Task.CompletedTask;
                }

                operation.Responses.TryAdd(
                    "401",
                    new() { Description = "Unauthorized" });
                operation.Responses.TryAdd(
                    "403",
                    new() { Description = "Forbidden" });

                var oAuthScheme = new OpenApiSecurityScheme
                {
                    Reference = new() { Type = ReferenceType.SecurityScheme, Id = "oauth2", }
                };

                operation.Security = [new() { [oAuthScheme] = scopes }];

                return Task.CompletedTask;
            });

        return options;
    }

    public static OpenApiOptions ApplyOperationDeprecatedStatus(this OpenApiOptions options)
    {
        options.AddOperationTransformer(
            (operation, context, _) =>
            {
                var apiDescription = context.Description;
                operation.Deprecated |= apiDescription.IsDeprecated();

                return Task.CompletedTask;
            });

        return options;
    }

    public static OpenApiOptions ApplySchemaNullableFalse(this OpenApiOptions options)
    {
        options.AddSchemaTransformer(
            (schema, _, _) =>
            {
                if (schema.Properties is null)
                    return Task.CompletedTask;

                foreach (var property in schema.Properties)
                {
                    if (schema.Required?.Contains(property.Key) != true)
                    {
                        property.Value.Nullable = false;
                    }
                }

                return Task.CompletedTask;
            });

        return options;
    }

    public class SecuritySchemeDefinitionsTransformer(IConfiguration configuration)
        : IOpenApiDocumentTransformer
    {
        public Task TransformAsync(OpenApiDocument document,
                                   OpenApiDocumentTransformerContext context,
                                   CancellationToken cancellationToken)
        {
            var identitySection = configuration.GetSection(nameof(Identity));

            if (!identitySection.Exists())
            {
                return Task.CompletedTask;
            }

            var identity = identitySection.Get<Identity>();

            var identityUrlExternal = identity?.Url;
            var scopes = identitySection
                         .GetRequiredSection("Scopes")
                         .GetChildren()
                         .ToDictionary(p => p.Key, p => p.Value);

            var securityScheme = new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.OAuth2,
                Flows = new()
                {
                    AuthorizationCode = new()
                    {
                        AuthorizationUrl =
                            new($"{identityUrlExternal}/realms/{nameof(FoodExpress)}/protocol/openid-connect/auth"),
                        TokenUrl = new(
                            $"{identityUrlExternal}/realms/{nameof(FoodExpress)}/protocol/openid-connect/token"),
                        Scopes = scopes,
                    }
                },
            };

            document.Components ??= new();
            document.Components.SecuritySchemes.Add("oauth2", securityScheme);

            return Task.CompletedTask;
        }
    }
}