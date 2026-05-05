using AuthorService.Api.GraphQL;
using AuthorService.Application.Interfaces;
using AuthorService.Domain.Interfaces;
using AuthorService.Infrastructure.Data;
using AuthorService.Infrastructure.Data.Repositories;
using HotChocolate.AspNetCore;
using Microsoft.EntityFrameworkCore;
using AuthorService.Application.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// Add GraphQL (HotChocolate)
builder.Services
    .AddGraphQLServer()
    .AddQueryType<AuthorQuery>() // Register GraphQL queries
    .AddType<AuthorType>()       // Register GraphQL types
    .AddFiltering()
    .AddSorting()
    .AddProjections();

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

// Add database context (EF Core)
builder.Services.AddPooledDbContextFactory<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("AuthorSchemaDB")));

// Add application services
builder.Services.AddScoped<IAuthorService, AuthorService.Application.Services.AuthorService>();

// Add infrastructure services
builder.Services.AddScoped<IAuthorRepository, AuthorRepository>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
   // app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors();
app.UseAuthorization();
app.MapControllers();
app.MapGraphQL("/graphql");
// Configure the HTTP request pipeline.

app.Run();
