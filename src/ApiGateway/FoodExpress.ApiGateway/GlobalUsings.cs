// Global using directives

global using System.Globalization;
global using System.IdentityModel.Tokens.Jwt;
global using System.Net.Http.Headers;
global using System.Security.Claims;
global using System.Threading.RateLimiting;
global using BuildingBlocks.Constants;
global using FoodExpress.ApiGateway.Extensions;
global using FoodExpress.ApiGateway.RateLimit;
global using FoodExpress.ServiceDefaults;
global using Microsoft.AspNetCore.Authentication;
global using Microsoft.AspNetCore.Authentication.Cookies;
global using Microsoft.AspNetCore.Authentication.OpenIdConnect;
global using Microsoft.AspNetCore.Authorization;
global using Microsoft.Extensions.Caching.Distributed;
global using Microsoft.Extensions.Options;
global using Microsoft.IdentityModel.JsonWebTokens;
global using Microsoft.IdentityModel.Protocols.OpenIdConnect;
global using Microsoft.IdentityModel.Tokens;
global using Yarp.ReverseProxy.Transforms;
