using Microsoft.EntityFrameworkCore;
using ProductService.Api.GraphQL.Queries;
using ProductService.Api.GraphQL.Mutations;
using ProductService.Api.GraphQL.Types;
using ProductService.Application.Interfaces;
using ProductService.Application.Services;
using ProductService.Infrastructure.Data;
using ProductService.Infrastructure.Data.Repositories;

var builder = WebApplication.CreateBuilder(args);

// DbContext (Pooled Context Factory is best practice for Hot Chocolate GraphQL)
var connectionString = builder.Configuration.GetConnectionString("ProductSchemaDB");

builder.Services.AddPooledDbContextFactory<ProductDbContext>(options =>
    options.UseSqlServer(connectionString));

// Allows traditional scoped resolution in repositories
builder.Services.AddScoped<ProductDbContext>(sp =>
    sp.GetRequiredService<IDbContextFactory<ProductDbContext>>().CreateDbContext());

// Repositories & Services
builder.Services.AddScoped<IProductReviewRepository, ProductReviewRepository>();
builder.Services.AddScoped<IProductReviewService, ProductReviewService>();

// GraphQL Setup
builder.Services
    .AddGraphQLServer()
    .AddQueryType<ProductReviewQuery>()
    .AddMutationType<ProductReviewMutation>()
    .AddType<ProductReviewType>()
    .AddType<ProductReviewImageType>()
    .AddType<ReviewHelpFulType>()
    .AddType<ReviewReaderTypeType>()
    .AddType<ReviewTagNameType>();



// Add CORS policy
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// Add Authorization services
builder.Services.AddAuthorization();

var app = builder.Build();

app.MapGet("/", () => "Product Service is running");

// it is important to have this before authorization if you have protected endpoints,
// otherwise it will redirect to HTTPS before checking authorization
app.UseHttpsRedirection();

// CORS policy - adjust as needed for your client applications
app.UseCors();

// Authorization middleware (if needed for protected endpoints)
app.UseAuthorization();

// Map GraphQL endpoint
app.MapGraphQL("/graphql");

app.Run();

