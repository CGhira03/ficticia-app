using Ficticia.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Ficticia.Application.Interfaces;
using Ficticia.Application.Services;
using Ficticia.API.Auth;
using Microsoft.AspNetCore.Authentication;
using Microsoft.OpenApi.Models;
using Ficticia.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

// --- Base de datos ---
builder.Services.AddDbContext<FicticiaDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// --- Servicios ---
builder.Services.AddScoped<IPersonService, PersonService>();

// --- Autenticación personalizada ---
builder.Services.AddAuthentication("FakeJwt")
    .AddScheme<AuthenticationSchemeOptions, JwtDemoHandler>("FakeJwt", null);

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
    options.AddPolicy("ReadOnly", policy => policy.RequireRole("Admin", "Consultor"));
});

// --- CORS ---
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policy =>
        {
            policy
                .WithOrigins("https://localhost:7090", "http://localhost:5074")
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});

// --- Controladores ---
builder.Services.AddControllers();

// --- Swagger ---
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "Ficticia API", Version = "v1" });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Ingrese el token: **Bearer admin** o **Bearer consultor**",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

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

// --- Seed de atributos ---
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<FicticiaDbContext>();
    SeedData.Initialize(context);
}

// --- Middleware ---
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowFrontend");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
