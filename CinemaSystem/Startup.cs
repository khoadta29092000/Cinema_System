using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccess.Repository;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;


namespace CinemaSystem
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(c =>
            {
                c.AddPolicy("AllowOrigin", options => options.AllowAnyOrigin().AllowAnyMethod()
                 .AllowAnyHeader());
            });

            services.AddControllers();
      
            services.AddSingleton<IAccountRepository, AccountRepository>();
            services.AddSingleton<ITypeRepository, TypeRepository>();
            services.AddSingleton<ICinemaRepository, CinemaRepository>();
            services.AddSingleton<IFilmRepository, FilmRepository>();
            services.AddSingleton<ITypeInFilmRepository, TypeInFilmRepository>();
            services.AddSingleton<IFilmInCinemaRepository, FilmInCinemaRepository>();
            services.AddSingleton<IServiceRepository, ServiceRepository>();
            services.AddSingleton<IServiceInCinemaRepository, ServiceInCinemaRepository>();
            services.AddSingleton<ILocationRepository, LocationRepository>();
            services.AddSingleton<IRoleRepository, RoleRepository>();
            services.AddSingleton<ICouponRepository, CouponRepository>();
            services.AddSingleton<IRoomRepository, RoomRepository>();
            services.AddSingleton<ISeatRepository, SeatRepository>();
            services.AddSingleton<ISchedulingRepository, SchedulingRepository>();
            services.AddSingleton<ITickedRepository, TickedRepository>();
            services.AddSingleton<IBillRepository, BillRepository>();
            services.AddSingleton<IServiceInBillRepository, ServiceInBillRepository>();

            services.AddControllers().AddNewtonsoftJson();

            var secretKey = Configuration["Appsettings:SecretKey"];
            var secretKeyBytes = Encoding.UTF8.GetBytes(secretKey);

            services.AddAuthorization();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer( opt =>
            {
                opt.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,

                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(secretKeyBytes),

                    ClockSkew = TimeSpan.Zero
                };
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "CinemaSystem", Version = "v1" });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "bearer"
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
                c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            app.UseCors(builder => builder
               .AllowAnyHeader()
               .AllowAnyMethod()
               .SetIsOriginAllowed((host) => true)
               .AllowCredentials().WithOrigins("https://localhost:5001",
                                               "https://localhost:3000",
                                               "http://cinemasystem.somee.com/swagger/index.html",
                                               "https://subscription-milk-delivery.herokuapp.com")
           );

            if (env.IsDevelopment() || env.IsProduction())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "CinemaSystem v1"));
            }
            app.UseAuthentication();

            app.UseAuthorization(); 

            app.UseHttpsRedirection();

            app.UseDeveloperExceptionPage();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
