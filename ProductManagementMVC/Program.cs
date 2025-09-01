using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using ProductManagementMVC.Areas.Identity.Data;
using ProductManagementMVC.Data;
using ProductManagementMVC.Interfaces;
using ProductManagementMVC.Mapping;
using ProductManagementMVC.Models.CategoryModels;
using ProductManagementMVC.Models.ProductModels;
using ProductManagementMVC.Models.OrderModels;
using ProductManagementMVC.Services;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("ProductManagementMVCContextConnection")
    ?? throw new InvalidOperationException("Connection string 'ProductManagementMVCContextConnection' not found.");

builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IMapper<ProductManagementMVC.Entities.Category, CategoryModel>, CategoryMapper>();

builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IMapper<ProductManagementMVC.Entities.Product, ProductModel>, ProductMapper>();

builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IMapper<ProductManagementMVC.Entities.Order, OrderModel>, OrderMapper>();

builder.Services.AddDbContext<ProductManagementMVCContext>(options =>
    options.UseSqlServer(connectionString));

// Identity + Roles
builder.Services.AddDefaultIdentity<ApplicationUser>(options =>
    options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()   //  როლების მხარდაჭერა
    .AddEntityFrameworkStores<ProductManagementMVCContext>();

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddTransient<IEmailSender, EmailSender>();

var app = builder.Build();

// Seed Roles & Admin User
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();

    string[] roleNames = { "Admin", "User" };
    foreach (var roleName in roleNames)
    {
        if (!await roleManager.RoleExistsAsync(roleName))
        {
            await roleManager.CreateAsync(new IdentityRole(roleName));
        }
    }

    // Admin User შექმნა ადმინის იუზერის
    var adminEmail = "admin@gmail.com";
    var adminPassword = "Admin123!";

    var adminUser = await userManager.FindByEmailAsync(adminEmail);
    if (adminUser == null)
    {
        adminUser = new ApplicationUser
        {
            UserName = adminEmail,
            Email = adminEmail,
            EmailConfirmed = true
        };
        var createResult = await userManager.CreateAsync(adminUser, adminPassword);
        if (createResult.Succeeded)
        {
            await userManager.AddToRoleAsync(adminUser, "Admin");
        }
    }
    else
    {
        if (!await userManager.IsInRoleAsync(adminUser, "Admin"))
        {
            await userManager.AddToRoleAsync(adminUser, "Admin");
        }
    }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// აუცილებელია Authentication წინ Authorization-ის
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

app.Run();











//using Microsoft.EntityFrameworkCore;
//using ProductManagementMVC.Areas.Identity.Data;
//using ProductManagementMVC.Data;
//using ProductManagementMVC.Interfaces;
//using ProductManagementMVC.Mapping;
//using ProductManagementMVC.Services;

//var builder = WebApplication.CreateBuilder(args);
//var connectionString = builder.Configuration.GetConnectionString("ProductManagementMVCContextConnection") ?? throw new InvalidOperationException("Connection string 'ProductManagementMVCContextConnection' not found.");

//builder.Services.AddScoped<ICategoryService, CategoryService>();
//builder.Services.AddScoped<IMapper<ProductManagementMVC.Entities.Category, ProductManagementMVC.Models.CategoryModel>, CategoryMapper>();

//builder.Services.AddDbContext<ProductManagementMVCContext>(options => options.UseSqlServer(connectionString));

//builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<ProductManagementMVCContext>();

//// Add services to the container.
//builder.Services.AddControllersWithViews();

//var app = builder.Build();

//// Configure the HTTP request pipeline.
//if (!app.Environment.IsDevelopment())
//{
//    app.UseExceptionHandler("/Home/Error");
//    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
//    app.UseHsts();
//}

//app.UseHttpsRedirection();
//app.UseStaticFiles();

//app.UseRouting();

//app.UseAuthorization();

//app.MapControllerRoute(
//    name: "default",
//    pattern: "{controller=Home}/{action=Index}/{id?}");

//app.MapRazorPages();

//app.Run();
