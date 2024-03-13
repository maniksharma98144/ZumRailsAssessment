﻿using Microsoft.AspNetCore.Diagnostics;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Server.Services;
using Server.Models;

namespace Server
{
    /// <summary>
    /// service extension for Program.cs
    /// </summary>
    public static class ServiceExtensions
    {
        /// <summary>
        /// extension method for configuring cors
        /// </summary>
        /// <param name="services"></param>
        /// <param name="corsStr"></param>
        public static void ConfigureCors(this IServiceCollection services, string corsStr)
        {
            services.AddCors(options =>
            {
                options.AddPolicy(corsStr,
                    builder => builder.WithOrigins("http://localhost:4200").
                    AllowAnyMethod().
                    AllowAnyHeader());
            });
        }

        /// <summary>
        /// extension method for configuring api versions
        /// </summary>
        /// <param name="services"></param>
        public static void ConfigureVersioning(this IServiceCollection services)
        {
            services.AddApiVersioning(options =>
            {
                options.ReportApiVersions = true;
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.ApiVersionReader = new UrlSegmentApiVersionReader();
            });
        }

        public static void ConfigureExceptionHandler(this IApplicationBuilder app, ILoggerService logger)
        {
            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    context.Response.ContentType = "application/json";
                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                    if (contextFeature != null)
                    {
                        logger.LogError($"Something went wrong: {contextFeature.Error}");
                        await context.Response.WriteAsync(new ErrorDetails()
                        {
                            StatusCode = context.Response.StatusCode,
                            Message = "Internal Server Error."
                        }.ToString());
                    }
                });
            });
        }
    }
}

