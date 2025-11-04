using Business.Interfaces;
using Business.Services;
using Data.Core;
using Data.Interfaces;
using Data.Repositories;
using Entity.Context;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Utilities.Mapping;
using Data.Init;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;


var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
var dbProvider = configuration["DatabaseProvider"];

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();


builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "API Tienda de Mascotas", Version = "v1" });

    var securityScheme = new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Introduce el token JWT en el campo Authorization",
        Reference = new OpenApiReference
        {
            Id = JwtBearerDefaults.AuthenticationScheme,
            Type = ReferenceType.SecurityScheme
        }
    };

    c.AddSecurityDefinition(securityScheme.Reference.Id, securityScheme);
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        { securityScheme, Array.Empty<string>() }
    });
});

// ============================================
// INYECCIÓN DE DEPENDENCIAS (REPOS + SERVICIOS)
// ============================================

// Repositorio genérico base
builder.Services.AddScoped(typeof(IRepository<>), typeof(GenericRepository<>));

// Repositorios y servicios específicos
builder.Services.AddScoped<IClienteRepository, ClienteRepository>();
builder.Services.AddScoped<ClienteService>();

builder.Services.AddScoped<IMascotaRepository, MascotaRepository>();
builder.Services.AddScoped<MascotaService>();

builder.Services.AddScoped<IProductoRepository, ProductoRepository>();
builder.Services.AddScoped<ProductoService>();

builder.Services.AddScoped<IVentaRepository, VentaRepository>();
builder.Services.AddScoped<VentaService>();

builder.Services.AddScoped<IPagoRepository, PagoRepository>();
builder.Services.AddScoped<PagoService>();

builder.Services.AddScoped<IMascotaClienteRepository, MascotaClienteRepository>();
builder.Services.AddScoped<MascotaClienteService>();

builder.Services.AddScoped<IVentaProductoRepository, VentaProductoRepository>();
builder.Services.AddScoped<VentaProductoService>();

// Autenticación propia del sistema
builder.Services.AddScoped<IAuthService, AuthService>();


var jwtKey = configuration["Jwt:Key"];
var keyBytes = Encoding.ASCII.GetBytes(jwtKey!);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(keyBytes),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});

builder.Services.AddMapster();
MapsterConfig.RegisterMappings();


builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    switch (dbProvider)
    {
        case "SqlServer":
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            break;
        case "MySql":
            options.UseMySql(configuration.GetConnectionString("MySqlConnection"),
                ServerVersion.AutoDetect(configuration.GetConnectionString("MySqlConnection")));
            break;
        case "Postgres":
            options.UseNpgsql(configuration.GetConnectionString("PostgresConnection"));
            break;
        default:
            throw new Exception("Proveedor de base de datos no soportado");
    }
});


var OrigenesPermitidos = builder.Configuration
    .GetValue<string>("OrigenesPermitidos")!
    .Split(",", StringSplitOptions.RemoveEmptyEntries);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularApp", policy =>
    {
        policy.WithOrigins(OrigenesPermitidos)
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});


var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowAngularApp");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();


using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<ApplicationDbContext>();
    DbInitializer.Initialize(context); // Ejecuta migraciones e inserta datos iniciales
}

app.Run();
