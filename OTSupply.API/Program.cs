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
using OTSupply.API.GraphQL.Queries;
using OTSupply.API.GraphQL.Mutations;
using HotChocolate;
using HotChocolate.AspNetCore;
using Microsoft.AspNetCore.Authentication;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "OTSupply API", Version = "v1" });
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
        policy.WithOrigins(["http://localhost:5173", "https://ordertosupplysrb.onrender.com"]) // Vite frontend
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

//builder.Services.AddDbContext<OTSupplyDbContext>(options =>
//options.UseSqlServer(builder.Configuration.GetConnectionString("OTSupplyConnectionString")));


builder.Services.AddDbContext<OTSupplyDbContext>(options =>
options.UseNpgsql(Environment.GetEnvironmentVariable("CONNECTION_STRING") ?? builder.Configuration.GetConnectionString("OTSupplyConnectionString")));

// ovde idu repozitoriji
builder.Services.AddScoped<ITokenRepository, TokenRepository>();
//

builder.Services.AddAutoMapper(typeof(AutoMapperProfiles));
builder.Services.AddHttpContextAccessor();

builder.Services.AddIdentity<Korisnik, IdentityRole>()
    .AddEntityFrameworkStores<OTSupplyDbContext>()
    .AddDefaultTokenProviders();

builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars = 0;
});

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
        NameClaimType = ClaimTypes.Name,
        RoleClaimType = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role",
        CryptoProviderFactory = new CryptoProviderFactory()
        {
            CacheSignatureProviders = false
        }
    };

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

// Add authorization policies
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Seller", policy =>
        policy.RequireRole("Seller"));
});

// --- GraphQL Server ---
builder.Services
    .AddGraphQLServer()
    .AddAuthorization()
    .AddQueryType<OglasQuery>()
    .AddMutationType<OglasMutation>()
    .AddProjections()
    .AddFiltering()
    .AddSorting();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

// MOVED DEBUG MIDDLEWARE BEFORE AUTHENTICATION
app.Use(async (context, next) =>
{
    Console.WriteLine($"\n\n=== NEW REQUEST: {context.Request.Path} ===");
    Console.WriteLine($"Auth Header: {context.Request.Headers["Authorization"]}");

    await next();

    Console.WriteLine($"Authenticated: {context.User.Identity?.IsAuthenticated}");
    Console.WriteLine($"User: {context.User.Identity?.Name}");

    if (context.User.Identity?.IsAuthenticated == true)
    {
        Console.WriteLine("Claims:");
        foreach (var claim in context.User.Claims)
        {
            Console.WriteLine($"{claim.Type}: {claim.Value}");
        }
    }

    Console.WriteLine($"Response: {context.Response.StatusCode}");
    Console.WriteLine("=== END REQUEST ===\n\n");
});

app.UseCors("AllowFrontend");
app.UseAuthentication();
app.UseAuthorization();

app.MapGraphQL("/graphql");
app.MapControllers();

app.Run();
