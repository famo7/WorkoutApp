using System.Diagnostics;
using System.Security.Claims;
using System.Text;
using System.Text.Json.Serialization;
using FluentValidation;
using GymApp.Endpoints;
using GymApp.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Npgsql;
using WorkoutApp;
using WorkoutApp.Endpoints;
using WorkoutApp.Repository;
using WorkoutApp.Repository.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddCors();
builder.Services.AddHttpContextAccessor();
builder.Services.AddTransient<IWorkoutRepository, WorkoutRepository>();
builder.Services.AddTransient<IExerciseRepository, ExerciseRepository>();

// builder.Services.AddSwaggerGen(c =>
// {
//     c.SwaggerDoc("v1", new OpenApiInfo
//     {
//         Title = "My API",
//         Version = "v1"
//     });
//     c.AddSecurityDefinition("bearer", new OpenApiSecurityScheme
//     {
//         In = ParameterLocation.Header,
//         Description = "Please insert JWT with bearer into field",
//         Name = "Authorization",
//         Type = SecuritySchemeType.ApiKey
//     });
//     c.AddSecurityRequirement(new OpenApiSecurityRequirement {
//    {
//      new OpenApiSecurityScheme
//      {
//        Reference = new OpenApiReference
//        {
//          Type = ReferenceType.SecurityScheme,
//          Id = "bearer"
//        }
//       },
//       new string[] { }
//     }
//   });
// });
var connString = "";



if (builder.Environment.IsDevelopment())
    connString = builder.Configuration.GetConnectionString("DefaultConnection");
else
{
    string connectionString = Environment.GetEnvironmentVariable("DATABASE_URL");

    Uri uri = new Uri(connectionString);

    string server = uri.Host;
    int port = uri.Port;
    string database = uri.AbsolutePath.TrimStart('/');
    string username = uri.UserInfo.Split(':')[0];
    string password = uri.UserInfo.Split(':')[1];

    connString = $"Server={server};Port={5432};User Id={username};Password={password};Database={database};";

}
builder.Services.Configure<Microsoft.AspNetCore.Http.Json.JsonOptions>(options =>
{
    options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});

builder.Services.AddDbContext<GymContext>(opt =>
{
    opt.UseNpgsql(connString);
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(
                 builder.Configuration.GetSection("TokenKey").Value!
            )),
            ValidateIssuer = false,
            ValidateAudience = false
        };

        options.Events = new JwtBearerEvents
        {
            OnAuthenticationFailed = context =>
            {
                context.Response.StatusCode = 401;
                return Task.CompletedTask;
            }
        };
    });
builder.Services.AddAuthorization(options =>
{
    options.FallbackPolicy = new AuthorizationPolicyBuilder()
    .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
    .RequireAuthenticatedUser()
    .Build();
});


builder.Services.AddValidatorsFromAssemblyContaining<Program>();

builder.Services.AddAutoMapper(typeof(MappingConfig));

var app = builder.Build();
app.UseCors(options => options.AllowAnyOrigin());



app.UseAuthentication();
app.UseAuthorization();

app.Use(async (context, next) =>
{
    var userName = context.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
    context.Items["UserName"] = userName;

    await next();
});


app.MapAccountEndpoints();
app.MapWorkoutEndpoints();
app.MapExerciseEndpoints();
app.MapSetEndpoints();

app.Run();
