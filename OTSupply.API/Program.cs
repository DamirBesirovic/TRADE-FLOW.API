using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using OTSupply.API.Data;
using OTSupply.API.Mappings;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Identity;
using OTSupply.API.Models.Domain;
using OTSupply.API.Repositories;
using Microsoft.OpenApi.Models;
using System.Security.Claims;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>  //sve ovo u options=> se koristi da moze da se radi autentifikacija i autorizacija preko swaggera
{
    options.SwaggerDoc("v1",new OpenApiInfo { Title="OTSupply API", Version="v1" });
    options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
    {
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = JwtBearerDefaults.AuthenticationScheme
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id= JwtBearerDefaults.AuthenticationScheme
                },
                Scheme="Oauth2",
                Name=JwtBearerDefaults.AuthenticationScheme,
                In=ParameterLocation.Header
            },
            new List<string>()
        }
    });
});
// cors za povezivanje sa frontom
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:5173") // Vite frontend
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

builder.Services.AddDbContext<OTSupplyDbContext>(options=>
options.UseSqlServer(builder.Configuration.GetConnectionString("OTSupplyConnectionString")));

// ovde idu repozitoriji
builder.Services.AddScoped<ITokenRepository,TokenRepository>();
//

builder.Services.AddAutoMapper(typeof(AutoMapperProfiles));
//builder.Services.AddIdentityCore<IdentityUser>()
//    .AddRoles<IdentityRole>()
//    .AddTokenProvider<DataProtectorTokenProvider<IdentityUser>>("OTSupply")
//    .AddEntityFrameworkStores<OTSupplyDbContext>()
//    .AddDefaultTokenProviders();

//
builder.Services.AddIdentity<Korisnik, IdentityRole>()
    .AddEntityFrameworkStores<OTSupplyDbContext>()
    .AddDefaultTokenProviders();
//


builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars = 0;
});


//builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//    .AddJwtBearer(options =>
//    options.TokenValidationParameters = new TokenValidationParameters
//    {
//        ValidateIssuer = true,
//        ValidateAudience = true,
//        ValidateLifetime = true,
//        ValidateIssuerSigningKey = true,
//        ValidIssuer = builder.Configuration["Jwt:Issuer"],
//        ValidAudience = builder.Configuration["Jwt:Audience"],
//        IssuerSigningKey = new SymmetricSecurityKey(
//            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))


//    });
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
        // These are CRITICAL for your setup:
        NameClaimType = ClaimTypes.Name,
        RoleClaimType = ClaimTypes.Role,
        // Add this to match your token's algorithm:
        CryptoProviderFactory = new CryptoProviderFactory()
        {
            CacheSignatureProviders = false
        }
    };

    // This fixes silent authentication failures:
    options.Events = new JwtBearerEvents
    {
        OnAuthenticationFailed = context =>
        {
            Console.WriteLine($"Authentication failed: {context.Exception}");
            return Task.CompletedTask;
        },
        OnTokenValidated = context =>
        {
            Console.WriteLine("Token validated successfully");
            return Task.CompletedTask;
        }
    };
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.UseCors("AllowFrontend"); // dodato za cors
app.UseAuthentication();
app.UseAuthorization();


app.Use(async (context, next) =>
{
    // Log the incoming request
    Console.WriteLine($"\n\n=== NEW REQUEST: {context.Request.Path} ===");

    // Log authentication status
    Console.WriteLine($"Authenticated: {context.User.Identity?.IsAuthenticated}");
    Console.WriteLine($"User: {context.User.Identity?.Name}");

    // Log ALL claims
    if (context.User.Identity?.IsAuthenticated == true)
    {
        Console.WriteLine("Claims:");
        foreach (var claim in context.User.Claims)
        {
            Console.WriteLine($"{claim.Type}: {claim.Value}");
        }
    }

    // Log the Authorization header
    Console.WriteLine($"Auth Header: {context.Request.Headers["Authorization"]}");

    await next();

    // Log the response
    Console.WriteLine($"Response: {context.Response.StatusCode}");
    Console.WriteLine("=== END REQUEST ===\n\n");
});

app.MapControllers();

app.Run();
