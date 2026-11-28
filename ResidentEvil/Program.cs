using Microsoft.EntityFrameworkCore;
using ResidentEvil.Services;
using ResidentEvil.Repositories;
using ResidentEvil.Data;
using Microsoft.AspNetCore.Identity;
using ResidentEvil.Services;

var builder = WebApplication.CreateBuilder(args);

// sqlite
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// repositories
builder.Services.AddScoped<IRepositoryWrapper, RepositoryWrapper>();
builder.Services.AddScoped<IEntryRepository, EntryRepository>();

// services
builder.Services.AddScoped<IEntryService, EntryService>();
builder.Services.AddScoped<IAuthService, AuthService>();


// login

builder.Services.AddIdentity<IdentityUser, IdentityRole>(options => {
    options.Password.RequireDigit = false;
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;

    options.Lockout.AllowedForNewUsers = true;
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();
builder.Services.AddControllersWithViews(options =>
{
    options.Filters.Add(new Microsoft.AspNetCore.Mvc.AutoValidateAntiforgeryTokenAttribute());
});

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.LogoutPath = "/Account/Logout";
    options.AccessDeniedPath = "/Account/AccessDenied";
});


var app = builder.Build();

// pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles(); // css

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();  

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// automigration
using (var dbScope = app.Services.CreateScope())
{
    var dbServices = dbScope.ServiceProvider;
    try
    {
        var dbContext = dbServices.GetRequiredService<ApplicationDbContext>();
        dbContext.Database.Migrate();
        Console.WriteLine(" --- Database Updated Successfully ---");
    }
    catch (Exception ex)
    {
        Console.WriteLine("An error occurred during migration: " + ex.Message);
    }
}

//  seed: ROLES + ADMIN ACCOUNT
using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

    foreach (var roleName in new[] { "Admin", "User" })
    {
        if (!await roleManager.RoleExistsAsync(roleName))
        {
            await roleManager.CreateAsync(new IdentityRole(roleName));
        }
    }

    var adminName = builder.Configuration["AdminSeed:UserName"];
    var adminPassword = builder.Configuration["AdminSeed:Password"];

    if (!string.IsNullOrWhiteSpace(adminName) && !string.IsNullOrWhiteSpace(adminPassword))
    {
        var admin = await userManager.FindByNameAsync(adminName);

        if (admin == null)
        {
            admin = new IdentityUser { UserName = adminName, Email = adminName };
            var created = await userManager.CreateAsync(admin, adminPassword);
            if (!created.Succeeded)
            {
                Console.WriteLine("Admin seed failed: " +
                    string.Join("; ", created.Errors.Select(e => e.Description)));
                admin = null;
            }
        }

        if (admin != null && !await userManager.IsInRoleAsync(admin, "Admin"))
        {
            await userManager.AddToRoleAsync(admin, "Admin");
        }
    }
}

app.Run();