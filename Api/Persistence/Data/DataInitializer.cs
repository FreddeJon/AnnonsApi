using Api.Application.Entities;
using Bogus;

namespace Api.Persistence.Data;

public static class DataInitializer
{
    public static async Task InitializeDataAsync(this IServiceProvider service)
    {
        await using var scope = service.CreateAsyncScope();

        var context = scope.ServiceProvider.GetService<ApplicationDbContext>();
        var userManager = scope.ServiceProvider.GetService<UserManager<IdentityUser>>();
        var roleManager = scope.ServiceProvider.GetService<RoleManager<IdentityRole>>();

        if (context is null) return;
        if (userManager is null) return;
        if (roleManager is null) return;


        await context.Database.MigrateAsync();


        await SeedRoleAsync(roleManager, "User");

        await userManager.SeedUserAsync("user@user.se", "user12345#", new[] { "User" });

        await context.SeedDataAsync();
    }


    private static async Task SeedUserAsync(this UserManager<IdentityUser> userManager, string email, string password, IEnumerable<string> roles)
    {
        var user = await userManager.FindByEmailAsync(email);

        if (user is null)
        {
            user = new IdentityUser()
            {
                Email = email,
                UserName = email,
                LockoutEnabled = false,
                PhoneNumberConfirmed = true,
                EmailConfirmed = true,
            };

            await userManager.CreateAsync(user, password);
            await userManager.AddToRolesAsync(user, roles);
        }
    }
    private static async Task SeedRoleAsync(this RoleManager<IdentityRole> roleManager, string role)
    {
        if (!await roleManager.RoleExistsAsync(role))
        {
            await roleManager.CreateAsync(new IdentityRole(role));
        }
    }
    private static async Task SeedDataAsync(this ApplicationDbContext context)
    {
        while (await context.Advertisements.CountAsync() < 100)
        {
            await context.Advertisements.AddAsync(GenerateAdvertisement());
            await context.SaveChangesAsync();
        }
    }
    private static Advertisement GenerateAdvertisement()
    {
        var ad = new Faker<Advertisement>("nb_NO")
            .StrictMode(false)
            .RuleFor(e => e.Id, f => 0)
            .RuleFor(e => e.Price, (f, u) => f.Finance.Amount(0, 1000))
            .RuleFor(e => e.PublishDate, (f, u) => DateTime.UtcNow)
            .RuleFor(e => e.Seller, (f, u) => f.Person.FullName)
            .RuleFor(e => e.Text, (f, u) => f.Lorem.Text())
            .RuleFor(e => e.Title, (f, u) => f.Lorem.Word());


        return ad.Generate(1).First();
    }
}

