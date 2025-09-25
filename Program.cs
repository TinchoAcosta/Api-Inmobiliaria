using inmobiliaria.Services;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Aca se agregan todoso los controladores y vistas
builder.Services.AddControllersWithViews();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>//el sitio web valida con cookie
    {
        options.LoginPath = "/Usuario/Login";
        options.LogoutPath = "/Usuario/Logout";
        options.AccessDeniedPath = "/Home";
        //options.ExpireTimeSpan = TimeSpan.FromMinutes(5);//Tiempo de expiración
    });
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Administrador", policy => policy.RequireRole("Administrador"));
});
// Registrar los repositorios
builder.Services.AddScoped<inmobiliaria.Models.RepositorioPropietario>();
builder.Services.AddScoped<inmobiliaria.Models.RepositorioInmueble>();
builder.Services.AddScoped<inmobiliaria.Models.RepositorioInquilino>();
builder.Services.AddScoped<inmobiliaria.Models.RepositorioContrato>();
builder.Services.AddScoped<inmobiliaria.Models.RepositorioPago>();
builder.Services.AddScoped<inmobiliaria.Models.RepositorioUsuario>();
builder.Services.AddScoped<inmobiliaria.Models.RepositorioTipoInmueble>();
builder.Services.AddScoped<inmobiliaria.Models.RepositorioAuditoria>();

builder.Services.AddScoped<AuditoriaHelper>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseStatusCodePagesWithReExecute("/Home/StatusCode", "?code={0}");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
