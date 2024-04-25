using FluentValidation;
using Market.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Market.Filters;

public class ExceptionFilter : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        if (context.Exception is DomainException domainException)
        {
            HandlerDomainException(domainException, context);
            return;
        }
        if (context.Exception is ValidationException validationException)
        {
            HandlerValidationException(validationException, context);
            return;
        }
    }

    private void HandlerDomainException(DomainException exception, ExceptionContext context)
    {
        context.Result = new JsonResult(new { Message = exception.Message })
        {
            StatusCode = exception.StatusCode
        };

        context.ExceptionHandled = true;
    }
    
    private void HandlerValidationException(ValidationException validationException, ExceptionContext context)
    {
        var errors = new List<object>();
        foreach (var validationResultError in validationException.Errors)
        {
            errors.Add(new
            {
                ErrorCode = validationResultError.ErrorCode,
                PropertyName = validationResultError.PropertyName,
                Message = validationResultError.ErrorMessage
            });
        }

        context.Result = new BadRequestObjectResult(new { Errors = errors });
        context.ExceptionHandled = true;
    }
}