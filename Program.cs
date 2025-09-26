using INMOBILIARIA__Oliva_Perez.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddSession();

// Repositorios
builder.Services.AddTransient<RepositorioUsuario>();
builder.Services.AddTransient<RepositorioContrato>();
builder.Services.AddTransient<RepositorioInmueble>();
builder.Services.AddTransient<RepositorioInquilino>();
builder.Services.AddTransient<RepositorioPropietario>();
builder.Services.AddTransient<RepositorioPago>();
builder.Services.AddTransient<RepositorioTipoInmueble>();

// Autenticación con cookie
builder.Services.AddAuthentication("CookieAuth")
    .AddCookie("CookieAuth", options =>
    {
        options.LoginPath = "/Usuario/Login";
        options.AccessDeniedPath = "/Usuario/AccesoDenegado";
    });

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseSession();

app.UseAuthentication(); 
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
