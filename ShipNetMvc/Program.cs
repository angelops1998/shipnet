using Microsoft.AspNetCore.Authentication.Cookies;
using ShipNetMvc.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

// El MVC no toca MySQL: todos los datos se los pide a ShipNetApi por HTTP
var baseUrl = builder.Configuration["ApiSettings:BaseUrl"];

builder.Services.AddHttpContextAccessor();
builder.Services.AddTransient<TokenHandler>();

builder.Services.AddHttpClient<IAuthService, AuthService>(client => client.BaseAddress = new Uri(baseUrl!));
builder.Services.AddHttpClient<IEquipoService, EquipoService>(client => client.BaseAddress = new Uri(baseUrl!))
    .AddHttpMessageHandler<TokenHandler>();
builder.Services.AddHttpClient<IMateriaService, MateriaService>(client => client.BaseAddress = new Uri(baseUrl!))
    .AddHttpMessageHandler<TokenHandler>();
builder.Services.AddHttpClient<IGrupoService, GrupoService>(client => client.BaseAddress = new Uri(baseUrl!))
    .AddHttpMessageHandler<TokenHandler>();
builder.Services.AddHttpClient<IDocenteService, DocenteService>(client => client.BaseAddress = new Uri(baseUrl!))
    .AddHttpMessageHandler<TokenHandler>();

// La sesión del navegador es una cookie; adentro guarda los roles y el token JWT de la API
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.AccessDeniedPath = "/Account/AccessDenied";
    });

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
