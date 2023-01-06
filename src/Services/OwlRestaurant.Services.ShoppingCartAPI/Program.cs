using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using OwlRestaurant.Services.ShoppingCartAPI.Abstractions.Repositories;
using OwlRestaurant.Services.ShoppingCartAPI.DBContexts;
using OwlRestaurant.Services.ShoppingCartAPI.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        options.Authority = builder.Configuration["ServiceURLs:IdentityAPI"];
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateAudience = false
        };
    });
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("APIPolicy", policy =>
    {
        policy.RequireAuthenticatedUser();
        policy.RequireClaim("scope", "owl");
    });
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "OwlRestaurant.Services.ShoppingCartAPI", Version = "v1" });
    c.EnableAnnotations();
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Description = "Enter Bearer token.",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {
            new OpenApiSecurityScheme() {
                Reference = new OpenApiReference()
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header,
            },
            new List<string>()
        }
    });
});

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("MSSQLConnection")));

builder.Services.AddAutoMapper(typeof(Program));

builder.Services.AddScoped<ICartRepository, CartRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
