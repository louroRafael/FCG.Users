using FCG.Users.Domain.Common;
using FCG.Users.Domain.Enums;
using FCG.Users.Domain.Interfaces.Common;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;

namespace FCG.Users.API.Filters;

public class ValidationFilter<T>(IAppLogger<ValidationFilter<T>> logger) : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var validator = context.HttpContext.RequestServices.GetService<IValidator<T>>();
        if (validator == null)
        {
            await next();
            return;
        }

        var request = context.ActionArguments.Values.OfType<T>().FirstOrDefault();
        if (request == null)
        {
            var errorResult = Result.Fail(new Error(
                ErrorType.Validation,
                "InvalidRequestType",
                $"Expected request of type {typeof(T).Name}."
            ));

            context.Result = new BadRequestObjectResult(errorResult);
            logger.LogError("ValidationFilter: Request type is null or not of expected type, expectedType: {expectedTypeName}", null, typeof(T).Name);
            return;
        }

        var validationResult = await validator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors
                .Select(e => new Error(ErrorType.Validation, e.PropertyName, e.ErrorMessage))
                .ToList();

            var errorResult = Result.Fail(errors.ToArray());

            context.Result = new BadRequestObjectResult(errorResult);

            logger.LogError(
                "ValidationFilter: Validation failed for request {requestTypeName}, errors: {validationErrors}",
                null,
                typeof(T).Name,
                string.Join(", ", errors.Select(e => e.Message))
            );
            return;
        }

        await next();
    }
}
