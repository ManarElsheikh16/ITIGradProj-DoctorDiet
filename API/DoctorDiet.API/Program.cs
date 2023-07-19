using Autofac;
using Autofac.Extensions.DependencyInjection;
using DoctorDiet.API;
using DoctorDiet.API.Config;
using DoctorDiet.Data;
using DoctorDiet.Models;
using DoctorDiet.Profiles;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Sakiny.Profiles;
using System.Diagnostics;
using System.Text;

namespace Sakiny.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            //Add services to the container.

            builder.Services.AddCors(options =>{
                options.AddPolicy("MyPolicy", policybuilder =>
                    //policybuilder.WithOrigins(builder.Configuration.GetSection("Cors").Value).AllowAnyMethod().AllowAnyHeader().AllowCredentials()
                    policybuilder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()
                );
                

            });

            builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

            builder.Host.ConfigureContainer<ContainerBuilder>(opt =>
                opt.RegisterModule(new AutoFacModule()));

            builder.Services.AddAutoMapper(typeof(RegisterAdminProfile).Assembly);
            builder.Services.AddAutoMapper(typeof(RegisterPatientProfile).Assembly);
            builder.Services.AddAutoMapper(typeof(RegisterDoctorProfile).Assembly);
            builder.Services.AddAutoMapper(typeof(NoteCreateProfile).Assembly);
            builder.Services.AddAutoMapper(typeof(UserDataProfile).Assembly);
            builder.Services.AddAutoMapper(typeof(NoteCreateProfile).Assembly);
            builder.Services.AddAutoMapper(typeof(NoEatProfile).Assembly);
            builder.Services.AddAutoMapper(typeof(ContactInfoProfile).Assembly);
            builder.Services.AddAutoMapper(typeof(MealProfile).Assembly);
            builder.Services.AddAutoMapper(typeof(DayProfile).Assembly);
            builder.Services.AddAutoMapper(typeof(PlanProfile).Assembly);
            builder.Services.AddAutoMapper(typeof(CustomPlanProfile).Assembly);
            builder.Services.AddAutoMapper(typeof(PatientNotesProfile).Assembly);



            builder.Services.AddDbContext<Context>(options =>
                 options.UseSqlServer(builder.Configuration.GetConnectionString("cs"))
                 .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
                 .LogTo(log => Debug.WriteLine(log), LogLevel.Information)
                 .EnableSensitiveDataLogging());

            builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
              .AddEntityFrameworkStores<Context>()
              .AddDefaultTokenProviders();

            builder.Services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = true; // Requires at least one digit
                options.Password.RequireLowercase = true; // Requires at least one lowercase letter
                options.Password.RequireUppercase = false; // Requires at least one lowercase letter

                options.Password.RequireNonAlphanumeric = false ;
                options.Password.RequiredLength = 8;

               options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+ اهعبتثجحخدذرزسشصضطظعغفقكلمنهويءآأؤإئىة";
              
            });

            builder.Services.AddAuthentication(
                options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                }).AddJwtBearer(options =>
                {
                    options.SaveToken = true;
                    options.RequireHttpsMetadata = false;
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuer = true,
                        ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
                        ValidateAudience = true,
                        ValidAudience = builder.Configuration["JWT:ValidAudience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:SecretKey"]))

                    };


                });
           

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.Configure<FormOptions>(options =>
            {
                options.MultipartBodyLengthLimit = 104857600;
            });

            //builder.Services.AddSwaggerGen();
            builder.Services.AddSwaggerGen(swagger =>
            {
              swagger.SwaggerDoc("v1", new OpenApiInfo
              {
                Version = "v1",
                Title = "ASP.NET 5 Web API",
                Description = " ITI Projrcy"
              });

              // Add this line to configure the file upload parameter
              //This is to generate the Default UI of Swagger Documentation    

              // To Enable authorization using Swagger (JWT)    
              swagger.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter 'Bearer' [space] and then your valid token in the text input below.\r\n\r\nExample: \"Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9\"",
                });
                swagger.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
new string[] { }

                    }
                });
            });


            var app = builder.Build();

            
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            //app.UseHttpsRedirection();

            app.UseCors("MyPolicy");//add header response Asscee-original-m
            app.UseAuthentication();//AddAuth==>jwt
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
