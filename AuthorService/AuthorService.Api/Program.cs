using AuthorService.Api.GraphQL;
using AuthorService.Application.Interfaces;
using AuthorService.Domain.Interfaces;
using AuthorService.Infrastructure.Data;
using AuthorService.Infrastructure.Data.Repositories;
using HotChocolate.AspNetCore;
using Microsoft.EntityFrameworkCore;
using AuthorService.Application.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Shared.Authentication.Extensions; 
var builder = WebApplication.CreateBuilder(args);

// Use shared authentication
builder.Services.AddSharedAuthentication(builder.Configuration);

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

// Add JWT Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
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
// Add authentication and authorization middleware
app.UseAuthentication(); // Ensure this is added before UseAuthorization
app.UseAuthorization();
app.MapControllers();

// Protect the GraphQL endpoint
app.MapGraphQL("/graphql").RequireAuthorization(); // Add RequireAuthorization to secure the endpoint
// Configure the HTTP request pipeline.

app.Run();
