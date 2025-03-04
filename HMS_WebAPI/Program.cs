using System.Text;
using HMS_Phase1;
using HMS_Phase1.Management_Classes;
using HMS_WebAPI.DbAccess;
using HMS_WebAPI.Managers;
using HMS_WebAPI.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        
        // Add services to the container.
        
        builder.Services.AddControllers();

        builder.Services.AddEndpointsApiExplorer();

        // Configure Swagger to support JWT authentication
        builder.Services.AddSwaggerGen(c =>
        {
            // Define the BearerAuth scheme that's in use
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "JWT Authorization header using the Bearer scheme. Enter 'Bearer' [space] and then your token.",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer"
            });


            // Add a global security requirement so that all endpoints require the token
            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        },
                        Scheme = "Bearer",
                        Name = "Bearer",
                        In = ParameterLocation.Header,
                    },
                    new List<string>()
                }
            });
        });

        builder.Services.AddScoped<AccountManager>();

        builder.Services.AddHttpContextAccessor();
        builder.Services.AddScoped<UserInfoService>();

        builder.Services.AddScoped<PatientRepository>();
        builder.Services.AddScoped<PatientManager>();

        builder.Services.AddScoped<BillingRepository>();
        builder.Services.AddScoped<BillingManager>();

        builder.Services.AddScoped<AppointmentRepository>();
        builder.Services.AddScoped<AppointmentManager>();

        builder.Services.AddScoped<DoctorRepository>();
        builder.Services.AddScoped<DoctorManager>();

        builder.Services.AddScoped<MedicationManager>();
        builder.Services.AddScoped<PrescriptionManager>();

        string connString = builder.Configuration.GetConnectionString("DefaultConnection");
        builder.Services.AddDbContext<HMSContext>(opt => opt.UseSqlServer(connString));

        string stringSecretKey = builder.Configuration["JwtSettings:SecretKey"];
        SecurityKey mySecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(stringSecretKey));

        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(opt =>
            {
                opt.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = mySecurityKey
                };
            });

        builder.Services.AddSingleton(builder.Configuration);

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseAuthentication();
        app.UseAuthorization(); 

        app.MapControllers();

        app.Run();


    }
}