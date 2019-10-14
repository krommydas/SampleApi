using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SampleApi.BusinessLogic
{
    public static class ErrorHandling
    {
        public static IApplicationBuilder UseErrorHandling(this IApplicationBuilder builder)
        {
            return builder.UseExceptionHandler((app) =>
            {
                app.Run(async context =>
                {
                    context.Response.ContentType = System.Net.Mime.MediaTypeNames.Text.Plain;

                    var exceptionHandlerPathFeature =
                        context.Features.Get<IExceptionHandlerPathFeature>();

                    context.Response.StatusCode = (int)System.Net.HttpStatusCode.InternalServerError;

                    if (exceptionHandlerPathFeature.Error is Storage.DuplicateException)
                    {
                        context.Response.StatusCode = (int)System.Net.HttpStatusCode.Conflict;
                        await context.Response.WriteAsync(exceptionHandlerPathFeature.Error.Message);
                    }
                    else if (exceptionHandlerPathFeature.Error is Storage.NotFoundException)
                    {
                        context.Response.StatusCode = (int)System.Net.HttpStatusCode.NotFound;
                        await context.Response.WriteAsync(exceptionHandlerPathFeature.Error.Message);
                    }
                    else if (exceptionHandlerPathFeature.Error is Storage.PersistenceException)
                    {
                        await context.Response.WriteAsync(exceptionHandlerPathFeature.Error.Message);
                    }
                    else
                        await context.Response.WriteAsync("something went wrong");
                });
            });
        }
    }
}
