using FluentValidation;
using TicketSupportSystemApp.ApiService.Services;
using TicketSupportSystemApp.ApiService.Interfaces;
using TicketSupportSystemApp.ApiService.Validators;
using TicketSupportSystemApp.ApiService.Data;
using Microsoft.AspNetCore.Identity;
using System.Text;
using TicketSupportSystemApp.ApiService.Data.Entities;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using TicketSupportSystemApp.ApiService.Common.Exceptions;

var builder = WebApplication.CreateBuilder(args);

// Add service defaults & Aspire client integrations.
builder.AddServiceDefaults();

var config = builder.Configuration;


string connection = config.GetConnectionString("DefaultConnection")??throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<TicketSupportSystemContext>(options => options.UseSqlServer(connection));

builder.Services.AddIdentity<User, Role>(o =>
    {
        o.Password.RequireDigit = false;
        o.Password.RequireLowercase = false;
        o.Password.RequireUppercase = false;
        o.Password.RequireNonAlphanumeric = false;
        o.User.RequireUniqueEmail = true;
    })
    .AddEntityFrameworkStores<TicketSupportSystemContext>()
    .AddDefaultTokenProviders();

builder.Services.AddAuthentication(opt =>
{
    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = config["Jwt:Issuer"],
        ValidAudience = config["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:SigningKey"] ?? throw new NotFoundException("JWT Signing Key not found.")))
    };
});


// Add services to the container.
builder.Services.AddProblemDetails();

builder.Services.AddValidatorsFromAssemblyContaining<CreateCommentValidator>();

builder.Services.AddScoped<ITicketsService, TicketsService>();
builder.Services.AddScoped<ICommentsService, CommentsService>();
builder.Services.AddScoped<IFileService, FileService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseExceptionHandler();

//if (app.Environment.IsDevelopment()){}

app.MapGet("/", () => "API service is running. Navigate to /weatherforecast to see sample data.");

app.MapDefaultEndpoints();

app.Run();

