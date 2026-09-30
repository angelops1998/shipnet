using Microsoft.AspNetCore.Authentication.Cookies;
using ShipNetMvc.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.Configure<Microsoft.AspNetCore.Http.Features.FormOptions>(options =>
{
    options.ValueLengthLimit = int.MaxValue;
    options.MultipartBodyLengthLimit = int.MaxValue;
    options.MemoryBufferThreshold = int.MaxValue;
});

// El MVC no toca MySQL: todos los datos se los pide a ShipNetApi por HTTP
var baseUrl = builder.Configuration["ApiSettings:BaseUrl"];

builder.Services.AddHttpContextAccessor();
builder.Services.AddTransient<TokenHandler>();

builder.Services.AddSingleton<INetworkService, NetworkService>();

builder.Services.AddHttpClient<IAuthService, AuthService>(client => client.BaseAddress = new Uri(baseUrl!));
builder.Services.AddHttpClient<IEquipoService, EquipoService>(client => client.BaseAddress = new Uri(baseUrl!))
    .AddHttpMessageHandler<TokenHandler>();
builder.Services.AddHttpClient<IMateriaService, MateriaService>(client => client.BaseAddress = new Uri(baseUrl!))
    .AddHttpMessageHandler<TokenHandler>();
builder.Services.AddHttpClient<IGrupoService, GrupoService>(client => client.BaseAddress = new Uri(baseUrl!))
    .AddHttpMessageHandler<TokenHandler>();
builder.Services.AddHttpClient<IDocenteService, DocenteService>(client => client.BaseAddress = new Uri(baseUrl!))
    .AddHttpMessageHandler<TokenHandler>();
builder.Services.AddHttpClient<IAsistenciaService, AsistenciaService>(client => client.BaseAddress = new Uri(baseUrl!))
    .AddHttpMessageHandler<TokenHandler>();
builder.Services.AddHttpClient<IEstudianteService, EstudianteService>(client => client.BaseAddress = new Uri(baseUrl!))
    .AddHttpMessageHandler<TokenHandler>();
builder.Services.AddHttpClient<IEvaluacionService, EvaluacionService>(client => client.BaseAddress = new Uri(baseUrl!))
    .AddHttpMessageHandler<TokenHandler>();
builder.Services.AddHttpClient<ICarreraService, CarreraService>(client => client.BaseAddress = new Uri(baseUrl!))
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
