using EVoucher_CMS_API.Helper;
using EVoucher_CMS_API.Models;
using EVoucher_CMS_API.Models.ViewModel.ResponseModels;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EVoucher_CMS_API.Extensions
{
    public static class ServiceConfiguration
    {
        //public static void ConfigDbContex(this IServiceCollection services, IConfiguration config)
        //{
        //    services.AddDbContext<EVoucherSystemDBContext>(options => options
        //    .UseSqlServer(config.GetConnectionString("EVoucherSystemDBString")));
        //}

        public static void ConfigCORS(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(
                    builder =>
                    {
                        builder.AllowAnyOrigin();
                        builder.AllowAnyHeader();
                        builder.AllowAnyMethod();
                    });
                options.AddPolicy("CorsPolicy",
                    builder =>
                    {
                        builder.WithExposedHeaders("X-Pagination");
                    });
            });
        }

        public static void ConfigJwtAuthorization(this IServiceCollection services, IConfiguration config)
        {
            services.AddHttpContextAccessor();
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = "JwtBearer";
                options.DefaultChallengeScheme = "JwtBearer";

            }).AddJwtBearer("JwtBearer", jwtBearerOptions =>
            {
                jwtBearerOptions.SecurityTokenValidators.Clear();
                jwtBearerOptions.SecurityTokenValidators.Add(new JwtValidationHandler(config));
                jwtBearerOptions.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = context =>
                    {
                        string errorStatus = "";
                        if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                        {
                            context.Response.Headers.Add("Token-Expired", "true");
                            errorStatus = "Token is Expired.";
                        }
                        else
                        {
                            errorStatus = "Invalid Token!";
                        }
                        Error err = new Error("Unauthorized Request", errorStatus);

                        var message = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(err));
                        context.Response.StatusCode = 401;
                        context.Response.ContentType = "application/json;";
                        context.Response.Body.WriteAsync(message, 0, message.Length);
                        return Task.CompletedTask;
                    }
                };
            });
        }
    }
}
