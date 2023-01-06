using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using OwlRestaurant.WebApp;
using OwlRestaurant.WebApp.Abstractions.Services;
using OwlRestaurant.WebApp.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient<IProductService, ProductService>();

builder.Services.AddScoped<IProductService, ProductService>();

builder.Services.AddControllersWithViews();

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
}).AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, c =>
{
    c.ExpireTimeSpan = TimeSpan.FromMinutes(10);
})
.AddOpenIdConnect(OpenIdConnectDefaults.AuthenticationScheme, c =>
{
    c.Authority = builder.Configuration["ServiceURLs:IdentityAPI"];
    c.GetClaimsFromUserInfoEndpoint = true;
    c.ClientId = "owl";
    c.ClientSecret = "testsecretchangethissecret";
    c.ResponseType = "code";
    c.TokenValidationParameters.NameClaimType = "name";
    c.TokenValidationParameters.RoleClaimType = "role";
    c.Scope.Add("owl");
    c.SaveTokens = true;
});

builder.Services.AddHttpContextAccessor();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
