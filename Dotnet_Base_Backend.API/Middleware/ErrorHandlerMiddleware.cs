using Dotnet_Base_Backend.API.Extensions;
using Dotnet_Base_Backend.Common.Errors;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using System.Threading.Tasks;
using System.Net;

namespace Dotnet_Base_Backend.API.Middleware
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        public async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            List<Error> errors = new List<Error>();
            Error error = null;
            string language = "en";
            if (!String.IsNullOrEmpty(context.Request.Headers["Accept-Language"].ToString()))
                language = context.Request.Headers["Accept-Language"].ToString();
            

            if (exception is RepositoryException)
            {
                RepositoryException repositoryException = (RepositoryException)exception;
                error = new Error(repositoryException.ErrorCode, repositoryException.GetErrorMessage(language) ,TypeErrors.REPOSITORY_ERROR, exception, context.Request.Method, context.Request.GetDisplayUrl());
                context.Response.StatusCode = (int)repositoryException.HttpStatusCode();
                errors.Add(error);
            }
            else if (exception is ServicesException)
            {
                ServicesException servicesException = (ServicesException)exception;
                error = new Error(servicesException.ErrorCode, servicesException.GetErrorMessage(language), TypeErrors.SERVICE_ERROR, exception, context.Request.Method, context.Request.GetDisplayUrl());
                context.Response.StatusCode = (int)servicesException.HttpStatusCode();
                errors.Add(error);
            }
            else if (exception is FluentValidation.ValidationException)
            {
                FluentValidation.ValidationException validationException = (FluentValidation.ValidationException)exception;
                foreach (var errorValidation in validationException.Errors)
                    errors.Add(new Error((ErrorCode)int.Parse(errorValidation.ErrorCode), errorValidation.GetErrorMessage(language) ,TypeErrors.VALIDATION_ERROR, exception, context.Request.Method, context.Request.GetDisplayUrl()));
                context.Response.StatusCode = (int)validationException.Errors.ElementAt(0).HttpStatusCode();
            }
            else
            {
                error = new Error(ErrorCode.INTERNAL_SERVER_ERROR, RepositoryException.GetErrorMessage(ErrorCode.INTERNAL_SERVER_ERROR), TypeErrors.UNKNOWN_ERROR, exception, context.Request.Method, context.Request.GetDisplayUrl());
                errors.Add(error);
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            }

            await context.Response.WriteAsJsonAsync(errors);
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class ErrorHandlerMiddlewareExtensions
    {
        public static IApplicationBuilder UseErrorHandlerMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ErrorHandlerMiddleware>();
        }
    }
}
