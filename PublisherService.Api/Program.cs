using Microsoft.EntityFrameworkCore;
using PublisherService.Api.Application.Interfaces;
using PublisherSvc = PublisherService.Api.Application.Services.PublisherService;
using PublisherService.Api.GraphQL.Queries;
using PublisherService.Api.GraphQL.Types;
using PublisherService.Api.Infrastructure.Data;
using PublisherService.Api.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// 1. DATABASE CONNECTION (Rule 3)
// We pull the connection string from appsettings.json
var connectionString = builder.Configuration.GetConnectionString("PublisherSchemaDB");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString));

// 2. DEPENDENCY INJECTION (Rule 2)
// Mapping our Interfaces to our Implementations
builder.Services.AddScoped<IPublisherRepository, PublisherRepository>();
builder.Services.AddScoped<IPublisherService, PublisherSvc>();

// 3. GRAPHQL SERVER SETUP
builder.Services
    .AddGraphQLServer()
    .AddQueryType<PublisherQuery>()
    .AddType<PublisherType>()
    .AddProjections() // Required for IQueryable to work
    .AddFiltering()   // Enables [UseFiltering]
    .AddSorting();     // Enables [UseSorting]

// 4. INFRASTRUCTURE SETUP
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(o => o.AddDefaultPolicy(p =>
    p.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()));

//// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
//builder.Services.AddOpenApi();

var app = builder.Build();

// 5. MIDDLEWARE PIPELINE
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();
app.MapControllers();

// The main entry point for our GraphQL Waiter
app.MapGraphQL("/graphql");

app.Run();
