using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using server.Data;
using server.Models;
using server.Repositories;
using server.Services;
using System.Text;

var wwwrootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
if (!Directory.Exists(wwwrootPath))
{
    Directory.CreateDirectory(wwwrootPath);
}

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// DB Context
builder.Services.AddDbContext<DB_Connect>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DB_Connect")));

// Identity configuration
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<DB_Connect>()
    .AddDefaultTokenProviders();

// JWT Authentication Configuration
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    var config = builder.Configuration;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = config["Jwt:Issuer"],
        ValidateAudience = true,
        ValidAudience = config["Jwt:Audience"],
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"])),
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
    };
});

// ✅ FIX: CORS → Ne pas utiliser "*" si AllowCredentials est activé
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy
            .WithOrigins("http://localhost:3000") // ✅ Met ton vrai frontend ici
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials(); // ✅ Nécessaire si React utilise withCredentials
    });
});

// Register TokenService
builder.Services.AddScoped<ITokenService, TokenService>();

builder.Services.AddScoped<IAdminServices, AdminServices>();

var app = builder.Build();

// Enable Swagger only in development
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage(); // (optionnel mais utile)
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseStaticFiles();

// Middleware order is important
app.UseCors("AllowFrontend");

if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection(); // ❗️Pas en dev
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
