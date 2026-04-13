using AuthorService.Api.GraphQL;
using AuthorService.Services;
using AuthorService.Data;
using AuthorService.GraphQL;

using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using AuthorSvc = AuthorService.Services.AuthorService;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



var conn = builder.Configuration.GetConnectionString("AuthorSchemaDB") ?? builder.Configuration.GetConnectionString("ConnectionStrings");
builder.Services.AddPooledDbContextFactory<ApplicationDbContext>(options =>
    options.UseSqlServer(conn));



builder.Services.AddScoped<IAuthorService, AuthorSvc>();


builder.Services
    .AddGraphQLServer()
    .AddQueryType<AuthorQuery>()
    .AddType<AuthorType>()
    .AddFiltering()
    .AddSorting()
    .AddProjections();

builder.Services.AddCors(o => o.AddDefaultPolicy(p => p.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()));


// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
//builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    //app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.MapGraphQL("/graphql");


app.Run();
