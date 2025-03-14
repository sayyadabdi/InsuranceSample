﻿using InsuranceSample.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace InsuranceSample.Handlers
{
    public class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            var problemDetails = new ProblemDetails();
            problemDetails.Instance = httpContext.Request.Path;
            if (exception is BaseException e)
            {
                httpContext.Response.StatusCode = (int)e.StatusCode;
                problemDetails.Title = e.Message;
            }
            else if (exception is FluentValidation.ValidationException)
            {
                httpContext.Response.StatusCode = (int)HttpStatusCode.UnprocessableEntity;
                problemDetails.Title = exception.Message;
            }
            else
            {
                problemDetails.Title = exception.Message;
            }
            logger.LogError("{ProblemDetailsTitle}", problemDetails.Title);
            problemDetails.Status = httpContext.Response.StatusCode;
            await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken).ConfigureAwait(false);
            return true;
        }
    }
}
