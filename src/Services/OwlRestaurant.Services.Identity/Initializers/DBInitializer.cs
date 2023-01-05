using IdentityModel;
using Microsoft.AspNetCore.Identity;
using OwlRestaurant.Services.Identity.Abstractions.Initializers;
using OwlRestaurant.Services.Identity.DBContexts;
using OwlRestaurant.Services.Identity.Models;
using System.Security.Claims;

namespace OwlRestaurant.Services.Identity.Initializers;

public class DBInitializer : IDBInitializer
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<ApplicationRole> _roleManager;

    public DBInitializer(ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
    {
        _context = context;
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public void Initialize()
    {
        if (_roleManager.FindByNameAsync(SD.Admin).Result == null)
        {            
            _roleManager.CreateAsync(new ApplicationRole()
            {
                Name = SD.Admin
            }).GetAwaiter().GetResult();
            _roleManager.CreateAsync(new ApplicationRole()
            {
                Name = SD.Customer
            }).GetAwaiter().GetResult();
        }
        else
        {
            return;
        }

        ApplicationUser adminUser = new ApplicationUser()
        {
            UserName = "admin@gmail.com",
            FirstName = "Bilal",
            LastName = "Demir",
            Email = "admin@gmail.com",
            EmailConfirmed = true,
            PhoneNumber = "1111111111",
            PhoneNumberConfirmed = true,
        };

        _userManager.CreateAsync(adminUser, "12345").GetAwaiter().GetResult();
        _userManager.AddToRoleAsync(adminUser, SD.Admin).GetAwaiter().GetResult();
        var adminUserClaims = _userManager.AddClaimsAsync(adminUser, new Claim[]
        {
            new Claim(JwtClaimTypes.Name, adminUser.FirstName + " " + adminUser.LastName),
            new Claim(JwtClaimTypes.GivenName, adminUser.FirstName),
            new Claim(JwtClaimTypes.FamilyName, adminUser.LastName),
            new Claim(JwtClaimTypes.Role, SD.Admin),
        }).Result;

        ApplicationUser customerUser = new ApplicationUser()
        {
            UserName = "customer@gmail.com",
            FirstName = "Demirhan",
            LastName = "Demir",
            Email = "customer@gmail.com",
            EmailConfirmed = true,
            PhoneNumber = "1111111111",
            PhoneNumberConfirmed = true,
        };

        _userManager.CreateAsync(customerUser, "12345").GetAwaiter().GetResult();
        _userManager.AddToRoleAsync(customerUser, SD.Customer).GetAwaiter().GetResult();
        var customerUserClaims = _userManager.AddClaimsAsync(customerUser, new Claim[]
        {
            new Claim(JwtClaimTypes.Name, customerUser.FirstName + " " + customerUser.LastName),
            new Claim(JwtClaimTypes.GivenName, customerUser.FirstName),
            new Claim(JwtClaimTypes.FamilyName, customerUser.LastName),
            new Claim(JwtClaimTypes.Role, SD.Customer),
        }).Result;
    }
}
