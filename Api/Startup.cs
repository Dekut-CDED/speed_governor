using System.Linq;
using System.Text;
using Api.Middleware;
using Application.User;
using Application.Interfaces;
using AutoMapper;
using Domain;
using FluentValidation.AspNetCore;
using Infrastructure.security;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using NSwag;
using Persistence;
using Infrastructure.Message;
using System;
using Serilog;
using Api.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;

namespace Api
{
    public class Startup
  {
    public Startup(IConfiguration configuration)
    {
      Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    // Configure development to use Sqlite.
    public void ConfigureDevelopmentServices(IServiceCollection services){
      services.AddDbContext<DataContext>(opt =>
      {
          opt.UseLazyLoadingProxies();
          opt.UseSqlite(Configuration.GetConnectionString("DefaultConnection"));
      });

         ConfigureServices(services);
        }
    public void ConfigureProductionServices(IServiceCollection services){
      services.AddDbContext<DataContext>(opt =>
      {
        opt.UseLazyLoadingProxies();
          opt.UseMySql(Configuration.GetConnectionString("DefaultConnection"), options => options.ServerVersion(new Version(8, 0, 19), Pomelo.EntityFrameworkCore.MySql.Infrastructure.ServerType.MySql).EnableRetryOnFailure()
        );
      });

         ConfigureServices(services);
        }

    public void ConfigureServices(IServiceCollection services)
    {
      services.AddCors();
      services.AddDbContext<DataContext>(opt =>
      {
          opt.UseLazyLoadingProxies();
          opt.UseSqlite(Configuration.GetConnectionString("DefaultConnection"));
      });

      // We will have a lot of handlers but we need to tell mediator once
      services.AddMediatR(typeof(Login.Handler).Assembly);
      services.AddAutoMapper(typeof(Login.Handler));

      services.AddControllers( opt =>
      {
          var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
          opt.Filters.Add(new AuthorizeFilter(policy));

      }).AddFluentValidation(
        cfg => {
          cfg.RegisterValidatorsFromAssemblyContaining<Login>();
        }
      );

      // configure Identity
      var builder = services.AddIdentityCore<AppUser>();
      var identitybuilder = new IdentityBuilder(builder.UserType, builder.Services);
      identitybuilder.AddEntityFrameworkStores<DataContext>();
      identitybuilder.AddSignInManager<SignInManager<AppUser>>();

      services.AddScoped<IJwtGenerator, JwtGenerator>();
      services.AddScoped<IUserAccessor, UserAccessor>();
      services.AddScoped<IMessage, MessageService>();

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Tokenkey"]));

      services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(opt =>
      {
          opt.TokenValidationParameters = new TokenValidationParameters
          {
              ValidateIssuerSigningKey = true,
              IssuerSigningKey = key,
              ValidateAudience = false,
              ValidateIssuer = false
          };
      });

        services.AddSwaggerDocument(document =>
            {
                document.Title = "Speed Governor";
                document.DocumentName = "v1";
                document.Description = " Speed governor data stream api";

                document.AddSecurity("Bearer", Enumerable.Empty<string>(), new NSwag.OpenApiSecurityScheme
                {
                    Type = OpenApiSecuritySchemeType.ApiKey,
                    Name = "Authorization",
                    Description = "copy 'Bearer ' + Valid jwt token into field",
                    In = OpenApiSecurityApiKeyLocation.Header
                });
            });

            services.AddApiVersioning(config =>
            {
                config.AssumeDefaultVersionWhenUnspecified = true;
                config.DefaultApiVersion = ApiVersion.Default;
                config.ReportApiVersions = true;
                config.ApiVersionReader = new HeaderApiVersionReader("api-version");
            });

        }


    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {

      app.UseOpenApi();
      app.UseSwaggerUi3();
            //added custom middleware.
       app.UseMiddleware<ErrorHandlingMiddleware>();
     // app.UseHttpsRedirection();

      app.UseRouting();
      app.UseSerilogRequestLogging();
      // use routing
      // Using routing
      app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

      app.UseAuthentication();
      app.UseAuthorization();

      app.UseEndpoints(endpoints =>
      {
        endpoints.MapControllers();
      });

    }
  }
}
