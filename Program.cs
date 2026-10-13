using internship.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
//This connects your C# application to the SQLite database.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(connectionString));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();
//This activates Identity, passwords, users, login, and roles.
builder.Services.AddDefaultIdentity<IdentityUser>(options =>
    options.SignIn.RequireConfirmedAccount = false)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.Configure<SecurityStampValidatorOptions>(options =>
{
    //This validates the user on every request. If a user is deleted, their old login cookie no longer keeps access.
    options.ValidationInterval = TimeSpan.Zero;
});    

builder.Services.AddControllersWithViews();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
//
app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.MapRazorPages()
    .WithStaticAssets();

using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider
        .GetRequiredService<RoleManager<IdentityRole>>();

    var userManager = scope.ServiceProvider
        .GetRequiredService<UserManager<IdentityUser>>();

    string[] roles = ["Read", "Write"];

    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
        {
            await roleManager.CreateAsync(new IdentityRole(role));
        }
    }

    const string writerEmail = "writer@internship.local";
    const string writerPassword = "Writer123!";

    var writerUser = await userManager.FindByEmailAsync(writerEmail);

    if (writerUser is null)
    {
        writerUser = new IdentityUser
        {
            UserName = writerEmail,
            Email = writerEmail,
            EmailConfirmed = true
        };

        var createResult = await userManager.CreateAsync(writerUser, writerPassword);

        if (!createResult.Succeeded)
        {
            var errors = string.Join(", ",
                createResult.Errors.Select(error => error.Description));

            throw new InvalidOperationException(
                $"Could not create the initial Write user: {errors}");
        }
    }

    if (!await userManager.IsInRoleAsync(writerUser, "Write"))
    {
        await userManager.AddToRoleAsync(writerUser, "Write");
    }
}

app.Run();