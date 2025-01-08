using FoodExpress.ServiceDefaults;
using FoodExpress.ServiceDefaults.OpenApi;
using Services.Ordering.Api.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.AddApplicationServices();

builder.AddDefaultOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapDefaultEndpoints();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet(
       "api/weatherforecast",
       () =>
       {
           var forecast = Enumerable.Range(1, 5).Select(
                                        index =>
                                            new WeatherForecast(
                                                DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                                                Random.Shared.Next(-20, 55),
                                                summaries[Random.Shared.Next(summaries.Length)]))
                                    .ToArray();

           return forecast;
       })
   .WithName("GetWeatherForecast")
   .RequireAuthorization();

app.UseApplicationServices();
app.UseDefaultOpenApi();

await app.RunAsync();

public record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
