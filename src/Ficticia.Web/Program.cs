using Ficticia.Web.Services; 

var builder = WebApplication.CreateBuilder(args);

// ---------------- CONFIGURACIÓN DE SERVICIOS ----------------

// Controladores y vistas
builder.Services.AddControllersWithViews();

// Para acceder al HttpContext (necesario para sesión y PersonService)
builder.Services.AddHttpContextAccessor();

// Configuración de sesión
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Registramos PersonService con HttpClient
builder.Services.AddHttpClient<PersonService>();

// Scoped para que siempre use el HttpContextAccessor actualizado
builder.Services.AddScoped<PersonService>();

var app = builder.Build();

// ---------------- CONFIGURACIÓN DE MIDDLEWARE ----------------

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Habilitar sesiones antes de cualquier acceso
app.UseSession();

// (Opcional) autenticación futura
app.UseAuthorization();

// Ruta por defecto
app.MapDefaultControllerRoute();

app.Run();
