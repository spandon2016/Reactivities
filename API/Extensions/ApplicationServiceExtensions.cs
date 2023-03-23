using Microsoft.OpenApi.Models;
using Persistence;
using Microsoft.EntityFrameworkCore;
using MediatR;
using Application.Activities;
using Application.Core;
using Application.Interfaces;
using Infrastructure.Security;
using Infrastructure.Photos;
using FluentValidation.AspNetCore;
using FluentValidation;

namespace API.Extensions
{
    public static class ApplicationServiceExtensions
    {
      

        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "WebAPIv5", Version = "v1" });
            });
            services.AddDbContext<DataContext>(options =>
            {
                var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

                string connStr;
                string connStr2;

                // Depending on if in development or production, use either FlyIO
                // connection string, or development connection string from env var.
                if (env == "Development")
                {
                    // Use connection string from file.
                    connStr = config.GetConnectionString("DefaultConnection");
                }
                else
                {
                    // Use connection string from file.
                    connStr = config.GetConnectionString("DefaultConnection");
                    connStr = "Server=reactivities-la-db.internal; Port=5432; User Id=postgres; Password=6bZXv1bD34yBwni; Database=reactivities";
                    Console.WriteLine("config in prod= {0}", connStr);
                    /*
                        
Postgres cluster reactivities-la-db created
  Username:    postgres
  Password:    6bZXv1bD34yBwni
  Hostname:    reactivities-la-db.internal
  Flycast:     fdaa:1:995a:0:1::7
  Proxy port:  5432
  Postgres port:  5433
  Connection string: postgres://postgres:6bZXv1bD34yBwni@reactivities-la-db.flycast:5432
  */

  //config in prod= Server=host.docker.internal; Port=5432; User Id=admin; Password=secret; Database=reactivities
  // Server=reactivities-la-db.internal; Port=5432; User Id=postgres; Password=6bZXv1bD34yBwni; Database=reactivities

                    //Console.WriteLine("{0}.{1}.{2}", mon, da, yer);
                    // Console.WriteLine($"{mon}.{da}.{yer}");  // note the $ prefix.
                

                    
                    // Use connection string provided at runtime by FlyIO.
                    var connUrl = Environment.GetEnvironmentVariable("DATABASE_URL");

                    // Parse connection URL to connection string for Npgsql
                    connUrl = connUrl.Replace("postgres://", string.Empty);
                    var pgUserPass = connUrl.Split("@")[0];
                    var pgHostPortDb = connUrl.Split("@")[1];
                    var pgHostPort = pgHostPortDb.Split("/")[0];
                    var pgDb = pgHostPortDb.Split("/")[1];
                    var pgUser = pgUserPass.Split(":")[0];
                    var pgPass = pgUserPass.Split(":")[1];
                    var pgHost = pgHostPort.Split(":")[0];
                    var pgPort = pgHostPort.Split(":")[1];

                    connStr2 = $"Server={pgHost};Port={pgPort};User Id={pgUser};Password={pgPass};Database={pgDb};";
                    Console.WriteLine("config in prod form env= {0}", connStr2);
                
                }

                // Whether the connection string came from the local development configuration file
                // or from the environment variable from FlyIO, use it to set up your DbContext.
                options.UseNpgsql(connStr);
            });

            services.AddCors(opt => {
                opt.AddPolicy("CorsPolicy", policy =>
                {
                    policy
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials()
                    .WithOrigins("http://localhost:3000");


                });
            });
            
            services.AddMediatR(typeof(List.Handler).Assembly);  
            //add mapper
            services.AddAutoMapper(typeof(MappingProfiles).Assembly);

            //add IUserAccessor
            services.AddScoped<IUserAccessor, UserAccessor>();

            services.AddScoped<IPhotoAccessor, PhotoAccessor>();

            services.Configure<CloudinarySettings>(config.GetSection("Cloudinary"));
            services.AddSignalR();

            services.AddFluentValidationAutoValidation();
            services.AddValidatorsFromAssemblyContaining<Create>();
            
            return services;


        }
    }
}