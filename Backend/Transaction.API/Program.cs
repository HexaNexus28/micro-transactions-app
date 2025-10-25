using Microsoft.EntityFrameworkCore;


// Ajoutez cette directive using
using Transaction.Business.Services;
using Transaction.Core.Interfaces.Repositories;
using Transaction.Core.Interfaces.Services;
using Transaction.Core.Mapping;
using Transaction.Data.Context;
using Transaction.Data.Repositories;
using Transaction.Data.UnitOfWork;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),
       b => b.MigrationsAssembly("Transaction.API")));

// Injection des dépendances - Repositories
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IAuthTokenRepository, AuthTokenRepository>();
builder.Services.AddScoped<ITransactRepository, TransactRepository>();
builder.Services.AddScoped<IItemRepository, ItemRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Injection des dépendances - Services
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IAuthTokenService, AuthTokenService>();
builder.Services.AddScoped<ITransactService, TransactService>();
builder.Services.AddScoped<IItemService, ItemService>();

// Configuration AutoMapper

builder.Services.AddAutoMapper(
    typeof(Program).Assembly,
    typeof(UserMappingProfile).Assembly,
      typeof(AuthTokenMappingProfile).Assembly,
      typeof(TransactMappingProfile).Assembly,
      typeof(ItemMappingProfile).Assembly

);

// Configuration des Controllers
builder.Services.AddControllers();

// Configuration Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Transaction API",
        Version = "v1",
        Description = "API pour la gestion des transactions"

    });
});
// Add services to the container.
// CORS (permettre toutes les origines en développement)
builder.Services.AddCors(options =>
{
    options.AddPolicy("DevelopmentPolicy",
        builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
});

builder.Services.AddHealthChecks();


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseCors("DevelopmentPolicy");
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
