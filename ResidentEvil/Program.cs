using Microsoft.EntityFrameworkCore;
using ResidentEvil.Services;
using ResidentEvil.Repositories;
using ResidentEvil.Data;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

// 1. Configurare Baza de Date (SQLite)
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// 2. Înregistrare REPOSITORIES
builder.Services.AddScoped<IRepositoryWrapper, RepositoryWrapper>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IGameRepository, GameRepository>();
builder.Services.AddScoped<IEntryRepository, EntryRepository>();

// 3. Înregistrare SERVICES
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IGamesService, GamesService>();
builder.Services.AddScoped<IEntryService, EntryService>();
builder.Services.AddScoped<IAuthService, AuthService>(); // Înregistrare AuthService

// 4. Adăugare suport pentru Controller + Views
builder.Services.AddControllersWithViews();

// login ---

builder.Services.AddIdentity<IdentityUser, IdentityRole>(options => {
    options.Password.RequireDigit = false;
    options.Password.RequiredLength = 3;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();
builder.Services.AddControllersWithViews(options =>
{
    options.Filters.Add(new Microsoft.AspNetCore.Mvc.AutoValidateAntiforgeryTokenAttribute());
});

// ---  ---
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.LogoutPath = "/Account/Logout";
    options.AccessDeniedPath = "/Account/AccessDenied";
});

var app = builder.Build();

// 6. Configurare PIPELINE (Middleware - Ordinea contează foarte mult aici!)
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles(); // Foarte important pentru CSS/JS/Imagini

app.UseRouting();

app.UseAuthentication(); // Pornire motor de autentificare (Identity)
app.UseAuthorization();  // Control acces roluri

// 7. DEFINIREA RUTELOR
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// 8. AUTOMIGRARE LA PORNIRE
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

//  --- CODE SEED PENTRU ROLURI AUTOMATE ---
using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    string[] roleNames = { "Admin", "User" };

    Task.Run(async () =>
    {
        foreach (var roleName in roleNames)
        {
            if (!await roleManager.RoleExistsAsync(roleName))
            {
                await roleManager.CreateAsync(new IdentityRole(roleName));
            }
        }
    }).GetAwaiter().GetResult();
}

app.Run();