using Ficticia.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Ficticia.Application.Interfaces;
using Ficticia.Application.Services;
using Ficticia.API.Auth;
using Microsoft.AspNetCore.Authentication;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// --- Base de datos ---
builder.Services.AddDbContext<FicticiaDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// --- Servicios ---
builder.Services.AddScoped<IPersonService, PersonService>();

// --- Autenticación personalizada ---
builder.Services.AddAuthentication("JwtDemo")
    .AddScheme<AuthenticationSchemeOptions, JwtDemoHandler>("JwtDemo", null);

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
    options.AddPolicy("ReadOnly", policy => policy.RequireRole("Admin", "Consultor"));
});

// --- Controladores ---
builder.Services.AddControllers();

// --- Swagger ---
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "Ficticia API", Version = "v1" });

    // 🔒 Definición de seguridad para el botón "Authorize"
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Ingrese el token: **Bearer admin** o **Bearer consultor**",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    // 🔐 Requerir seguridad globalmente
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

var app = builder.Build();

// --- Middleware ---
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// 🔒 Importante: el orden de estos dos middlewares
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
