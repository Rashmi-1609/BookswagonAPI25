using CategoryService.Api.GraphQL.Queries;
using CategoryService.Api.GraphQL.Types;

//using CategoryService.Api.GraphQL.Types;
using CategoryService.Application.Interfaces;
using CategoryService.Application.Services;

using CategoryService.Domain.Interfaces;
using CategoryService.Infrastructure.Data;
using CategoryService.Infrastructure.Data.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

// Add GraphQL (HotChocolate)
builder.Services
    .AddGraphQLServer()
    .AddQueryType<CategoryQuery>()
    .AddType<CategoryHierarchyType>() // Registers the wrapper
    .AddType<TopLevelCategoryType>()
    .AddType<SecondLevelCategoryType>()
    .AddType<ThirdLevelCategoryType>()
    .AddType<FourthLevelCategoryType>()
    .AddType<FlatMenuCategoryType>()
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
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("CategorySchema")));

// Add application services
builder.Services.AddScoped<ICategoryService, CategoryService.Application.Services.CategoryService>();

// Add infrastructure services
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();

var app = builder.Build();

//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

app.UseHttpsRedirection();
app.UseCors();
app.UseAuthorization();
app.MapControllers();
app.MapGraphQL("/graphql"); //middleware for GraphQL endpoint

app.Run();