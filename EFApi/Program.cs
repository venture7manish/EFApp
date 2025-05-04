using EFApi.Middleware;
using EFData.Data;
using EFData.Models;
using EFData.Repositories;
using EFServices.Interfaces;
using EFServices.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddOpenApi();

// Add DbContext with SQL Server connection string (adjust according to your environment)
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("dbcs"),
        sqlOptions => sqlOptions.MigrationsAssembly("EFData")));

// Register repositories
builder.Services.AddScoped<IStudentRepository, StudentRepository>();

// Register services
builder.Services.AddScoped<IStudentService, StudentService>();

// Configure JWT authentication
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = jwtSettings.GetValue<string>("SecretKey") ?? "ThisIsASecretKeyForDevelopment";

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false, // for demo purpose
        ValidateAudience = false,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
    };
});

builder.Services.AddControllers(options =>
{
    options.Filters.Add<CustomValidationFilter>();
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = @"JWT Authorization header using the Bearer scheme. 
                      Enter 'Bearer' [space] and then your token in the text input below.
                      Example: 'Bearer abc123'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Scheme = "Bearer",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
      {
        new OpenApiSecurityScheme
        {
          Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer"}
        },
        new string[] {}
      }
    });
});


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    // in case you need to redirect directly to Swagger API when trying to hit root URL, uncomment the below line.
    //app.MapGet("/", () => Results.Redirect("/swagger"));
}
//app.UseSwagger();
//app.UseSwaggerUI();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.UseHttpsRedirection();
app.Run();

public class CustomValidationFilter : Microsoft.AspNetCore.Mvc.Filters.IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(Microsoft.AspNetCore.Mvc.Filters.ActionExecutingContext context, Microsoft.AspNetCore.Mvc.Filters.ActionExecutionDelegate next)
    {
        if (!context.ModelState.IsValid)
        {
            context.Result = new Microsoft.AspNetCore.Mvc.BadRequestObjectResult(context.ModelState);
            return;
        }
        await next();
    }
}