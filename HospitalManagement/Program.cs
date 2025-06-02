using HospitalManagement.Data;
using HospitalManagement.Helpers;
using HospitalManagement.Helpers.Interface;
using HospitalManagement.Interface;
using HospitalManagement.Models.Mails;
using HospitalManagement.Repository;
using HospitalManagement.Repository.Interface;
using HospitalManagement.Services;
using HospitalManagement.Services.Interface;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

//builder.WebHost.UseUrls("http://0.0.0.0:5000");

//logging 
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .WriteTo.Console()
    .WriteTo.File("Logs/hopitalmanagementLogs.txt")
    .CreateLogger();

//builder.Host.UseSerilog();
// Add services to the container.

builder.Services.AddControllers( options =>
{
    options.Filters.Add<LoggingActionFilter>();
} );
    
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


string connetionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connetionString));

builder.Services.AddScoped(typeof(IRepository<>) , typeof(GenericRepository<>));
//builder.Services.AddScoped<>
builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("MailSettings")); ;
builder.Services.AddTransient<IEmailService, EmailSendService>();

builder.Services.AddScoped<IDepartmentService, DepartmentService>();
builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>();
builder.Services.AddScoped<IPatientRepository, PatientRepository>();
builder.Services.AddScoped<IPatientService, PatientService>();
builder.Services.AddScoped<IDoctorRepository, DoctorRepository>();
builder.Services.AddScoped<IDoctorService, DoctorService>();
builder.Services.AddScoped<IAppointmentRepository, AppointmentRepository>();
builder.Services.AddScoped<IAppointmentService ,AppointmentService>();

builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddScoped<ILeaveRepository, LeaveManagementRepository>();
builder.Services.AddScoped<IServiceLeaveManagement, LeaveService>();

builder.Services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
builder.Services.AddScoped<IAuthService, AuthService>();



//builder.Logging.SetMinimumLevel(LogLevel.Information);

var jwtSettings = builder.Configuration.GetSection("Jwt");

// Configure JWT Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings["Issuer"],
            ValidAudience = jwtSettings["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(jwtSettings["Key"])),
            RoleClaimType = ClaimTypes.Role
        };
       
    });


builder.Services.AddSwaggerGen( c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo
        {
            Title = "JWTToken_Auth_API",
            Version = "v1"
        });
        c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
        {
            Name = "Authorization",
            Type = SecuritySchemeType.ApiKey,
            Scheme = "Bearer",
            BearerFormat = "JWT",
            In = ParameterLocation.Header,
            Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 1safsfsdfdfd\"",
        });
        c.AddSecurityRequirement(new OpenApiSecurityRequirement {
        {
            new OpenApiSecurityScheme {
                Reference = new OpenApiReference {
                    Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseMiddleware<GlobalExceptionMiddleware>();
app.UseHttpsRedirection( );

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();



