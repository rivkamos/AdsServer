using AdvertisingAds.Api.Middlewares;
using AdvertisingAds.Core.Interfaces;
using AdvertisingAds.Core.Models;
using AdvertisingAds.Service.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Advertisung.Repository;

var builder = WebApplication.CreateBuilder(args);

var jwtSecret = builder.Configuration["JwtSettings:SecretKey"]; 
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
        ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
        ValidAudience = builder.Configuration["JwtSettings:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret))
    };
});
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IAdService, AdService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<UserRepository>();
builder.Services.AddCors(o => o.AddPolicy("MyPolicy", builder =>
{
    builder.AllowAnyOrigin()
           .AllowAnyMethod()
           .AllowAnyHeader();
}));


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    //app.UseExceptionHandler("/Error");
    app.UseSwagger();
    app.UseSwaggerUI();
}
//app.UseMiddleware<ExceptionHandlingMiddleware>(); 
app.UseHttpsRedirection();
app.UseCors("MyPolicy");
app.UseAuthentication(); 
app.UseAuthorization();

app.MapControllers();

app.Run();
