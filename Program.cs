var builder = WebApplication.CreateBuilder(args);

// Aca se agregan todoso los controladores y vistas
builder.Services.AddControllersWithViews();

// Registrar los repositorios
builder.Services.AddScoped<inmobiliaria.Models.RepositorioPropietario>();
builder.Services.AddScoped<inmobiliaria.Models.RepositorioInmueble>();
builder.Services.AddScoped<inmobiliaria.Models.RepositorioInquilino>();
builder.Services.AddScoped<inmobiliaria.Models.RepositorioContrato>();
builder.Services.AddScoped<inmobiliaria.Models.RepositorioPago>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
