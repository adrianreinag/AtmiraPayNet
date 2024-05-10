using AtmiraPayNet.Server.Context;
using AtmiraPayNet.Server.Models;
using AtmiraPayNet.Server.Services;
using AtmiraPayNet.Server.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer("name=DefaultConnection"));

builder.Services.AddIdentity<User, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

builder.Services.AddScoped<IApplicationDbContext, ApplicationDbContext>();

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IPaymentService, PaymentService>();
builder.Services.AddScoped<IPDFService, PDFService>();
builder.Services.AddScoped<IRESTCountriesService, RESTCountriesService>();

builder.Services.AddHttpClient<IRESTCountriesService, RESTCountriesService>();

builder.Services.AddHttpContextAccessor();

builder.Configuration.AddJsonFile("appsettings.Development.json");
var secretKey = builder.Configuration.GetSection("settings").GetSection("secretKey").ToString();
var keyBytes = Encoding.UTF8.GetBytes(secretKey!);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.SaveToken = true;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(keyBytes),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                });

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "AtmiraPayNet",
        Description = "API desarrollada para la aplicación web AtmiraPayNet"
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1");
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
