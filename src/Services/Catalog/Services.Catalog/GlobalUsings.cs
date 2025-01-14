// Global using directives

global using System.Text.Json;
global using Ardalis.GuardClauses;
global using Ardalis.Result;
global using BuildingBlocks.Constants;
global using BuildingBlocks.Core.Commands;
global using BuildingBlocks.Core.Domain;
global using BuildingBlocks.Core.Domain.Abstractions;
global using BuildingBlocks.Core.Queries;
global using BuildingBlocks.SharedKernel.ActivityScope;
global using BuildingBlocks.SharedKernel.ApiVersioning;
global using BuildingBlocks.SharedKernel.Clock;
global using BuildingBlocks.SharedKernel.Identity;
global using BuildingBlocks.SharedKernel.Metrics;
global using BuildingBlocks.SharedKernel.Pipelines;
global using FoodExpress.ServiceDefaults;
global using Microsoft.AspNetCore.Builder;
global using Microsoft.EntityFrameworkCore;
global using Microsoft.EntityFrameworkCore.Metadata.Builders;
global using Microsoft.Extensions.DependencyInjection;
global using Microsoft.Extensions.Hosting;
global using Microsoft.Extensions.Logging;
global using Services.Catalog.Domain.BrandAggregate;
global using Services.Catalog.Domain.CategoriesAggregate;
global using Services.Catalog.Domain.SupplierAggregate;
global using Services.Catalog.Infrastructure.Data;
