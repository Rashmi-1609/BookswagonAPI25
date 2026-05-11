using Microsoft.EntityFrameworkCore;
using PublisherService.Api.GraphQL.Filters;
using PublisherService.Api.GraphQL.Queries;
using PublisherService.Api.GraphQL.Types;
using PublisherService.Application.Interfaces;
using PublisherSvc = PublisherService.Application.Services.PublisherService;
using PublisherService.Domain.Interfaces;
using PublisherService.Infrastructure.Data;
using PublisherService.Infrastructure.Data.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add database context (EF Core)
builder.Services.AddPooledDbContextFactory<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("PublisherSchemaDB")));
// This tells the DI system: "When the Repository asks for ApplicationDbContext, 
// resolve the Factory, create a pooled context, and give it to them."
builder.Services.AddScoped<ApplicationDbContext>(sp =>
    sp.GetRequiredService<IDbContextFactory<ApplicationDbContext>>().CreateDbContext());

// Add infrastructure services
builder.Services.AddScoped<IPublisherRepository, PublisherRepository>();

// Add application services
builder.Services.AddScoped<IPublisherService, PublisherSvc>();

builder.Services
    .AddGraphQLServer()
    .AddQueryType(d => d.Name("Query"))    // Creates a base Query root
    .AddTypeExtension<PublisherQuery>()    // Registers our GET endpoints
    .AddType<PublisherType>()             // Registers our Entity/DTO schema definitions
    .AddProjections()                     // Enables [UseProjection]
    .AddFiltering()                       // Enables [UseFiltering]
    .AddSorting()                         // Enables [UseSorting]
    .AddErrorFilter<GraphQLErrorFilter>();// Hires our Dedicated PR Person to catch crashes

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

var app = builder.Build();

app.UseCors();
//app.UseAuthorization();
app.MapGraphQL("/graphql");

app.Run();
