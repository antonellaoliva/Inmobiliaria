using INMOBILIARIA__Oliva_Perez.Models;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllersWithViews();


builder.Services.AddTransient<RepositorioInmueble>();
builder.Services.AddTransient<RepositorioPropietario>();
builder.Services.AddTransient<RepositorioInquilino>();
builder.Services.AddTransient<RepositorioContrato>();
// builder.Services.AddTransient<RepositorioPago>();
// builder.Services.AddTransient<RepositorioUsuario>();


var app = builder.Build();


if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles(); 

app.UseRouting();

app.UseAuthorization();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();



