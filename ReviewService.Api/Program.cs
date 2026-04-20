using Microsoft.EntityFrameworkCore;
using ReviewService.Api.Application.Interfaces;
using ReviewService.Api.Application.Services;
using ReviewService.Api.GraphQL.Queries;
using ReviewService.Api.Infrastructure.Data;
using ReviewService.Api.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// 1. Setup Database Context by adding AddDbContext
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? builder.Configuration.GetConnectionString("ReviewSchemaDB");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString));

// 2. Dependency Injection
// Infrastructure Layer
builder.Services.AddScoped<IProductReviewRepository, ProductReviewRepository>();
// Application Layer
builder.Services.AddScoped<IProductReviewService, ProductReviewService>();

// 3. Hot Chocolate GraphQL Setup
builder.Services
    .AddGraphQLServer()
    .AddQueryType<ProductReviewQuery>()
    // These allow the client to use filtering/sorting if we add them later
    .AddFiltering()
    .AddSorting()
    .AddProjections();

var app = builder.Build();

// Enable the Banana Cake Pop playground and the GraphQL endpoint
app.MapGraphQL("/graphql");

app.Run();
