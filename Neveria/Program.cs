using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Neveria.Models;
using Neveria.Models.dbFreezeDream;
using Neveria.Services;

var builder = WebApplication.CreateBuilder(args);

// Agregar servicios MVC
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<DbFreezeDreamContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("dbContext")));

builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IInventoryService, InventoryService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ISaleService, SaleService>();

var app = builder.Build();

// Manejo de errores en producción
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();   // ← Necesario para que carguen los CSS de wwwroot

app.UseRouting();

app.UseAuthorization();

// ↓ RUTA POR DEFECTO: abre Login al entrar a localhost
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Login}/{id?}"); // ← Login como página de inicio

app.Run();