using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Net;

namespace Core.Web.Filters
{
    public class ErrorResponseExceptionFilter : ExceptionFilterAttribute
    {
        protected IHostingEnvironment Environment { get; }
        public ErrorResponseExceptionFilter(IHostingEnvironment env)
        {
            Environment = env;
        }

        public override void OnException(ExceptionContext context)
        {
            // Handle exceptions
            ApiError apiError = null;
            if(context.Exception is ArgumentException)
            {
                // API exceptions are mapped to HTTP 400 BadRequest
                var exception = context.Exception as ArgumentException;
                context.Exception = null;
                apiError = new ApiError(exception.Message)
                {
                    ErrorCode = (int) HttpStatusCode.BadRequest,
                    StackTrace = Environment.IsDevelopment() ? exception.StackTrace : null
                };

                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            }
            else if (context.Exception is ApiException)
            {
                // API exceptions are mapped to HTTP 400 BadRequest
                var exception = context.Exception as ApiException;
                context.Exception = null;
                apiError = new ApiError(exception.Message)
                {
                    ErrorCode = exception.ErrorCode,
                    StackTrace = Environment.IsDevelopment() ? exception.StackTrace : null
                };

                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            }
            else if (context.Exception is UnauthorizedAccessException)
            {
                // Unauthorized access is mapped to HTTP 401 Unauthorized
                apiError = new ApiError("Unauthorized access");
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            }
            else if(context.Exception is TooManyRequestException)
            {
                // too many request exceptions are mapped to HTTP 429 RequestRateTooLarge
                var exception = context.Exception as TooManyRequestException;
                context.Exception = null;
                apiError = new ApiError(exception.Message)
                {
                    ErrorCode = 429,
                    StackTrace = Environment.IsDevelopment() ? exception.StackTrace : null
                };

                context.HttpContext.Response.StatusCode = 429;
            }
            else
            {
                // By default, errors are mapped to HTTP 500 Internal Server Error
                var exception = context.Exception;
                apiError = new ApiError(Environment.IsDevelopment() ? exception.Message : "An unexpected error occurred.")
                {
                    StackTrace = Environment.IsDevelopment() ? exception.StackTrace : null
                };

                // Return default error response
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            }

            context.Result = new JsonResult(apiError);
            base.OnException(context);
        }
    }
}
