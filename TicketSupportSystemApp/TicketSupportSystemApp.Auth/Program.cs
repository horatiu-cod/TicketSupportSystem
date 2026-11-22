using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TicketSupportSystemApp.Auth.Data;
using TicketSupportSystemApp.Auth.Data.Entities;
using TicketSupportSystemApp.Auth.Interfaces;
using TicketSupportSystemApp.Auth.Services;
using TicketSupportSystemApp.Auth.Validators;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;

string connection = config.GetConnectionString("DefaultConnection")??throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<TicketSupportSystemAuthContext>(options => options.UseSqlServer(connection));

builder.Services.AddIdentity<User, Role>(o =>
    {
        o.Password.RequireDigit = false;
        o.Password.RequireLowercase = false;
        o.Password.RequireUppercase = false;
        o.Password.RequireNonAlphanumeric = false;
        o.User.RequireUniqueEmail = true;
    })
    .AddEntityFrameworkStores<TicketSupportSystemAuthContext>()
    .AddDefaultTokenProviders();

builder.Services.AddValidatorsFromAssemblyContaining<LoginValidator>();
builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();

var app = builder.Build();

app.UseHttpsRedirection();


app.Run();
