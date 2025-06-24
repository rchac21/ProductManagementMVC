using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ProductManagementMVC.Areas.Identity.Data;
using ProductManagementMVC.Data;
using ProductManagementMVC.Interfaces;
using ProductManagementMVC.Mapping;
using ProductManagementMVC.Services;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("ProductManagementMVCContextConnection") ?? throw new InvalidOperationException("Connection string 'ProductManagementMVCContextConnection' not found.");

builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IMapper<ProductManagementMVC.Entities.Category, ProductManagementMVC.Models.CategoryModel>, CategoryMapper>();

builder.Services.AddDbContext<ProductManagementMVCContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<ProductManagementMVCContext>();

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

app.Run();
