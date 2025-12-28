using FCG.Users.Domain.Common;
using FCG.Users.Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace FCG.Users.API.Extensions
{
    public static class ResultExtensions
    {
        public static IActionResult ToActionResult<T>(this Result<T> result)
        {
            if (result.Success)
                return new OkObjectResult(result);

            var statusCode = result.Errors.First().Type switch
            {
                ErrorType.Validation => StatusCodes.Status400BadRequest,
                ErrorType.Unauthorized => StatusCodes.Status401Unauthorized,
                ErrorType.NotFound => StatusCodes.Status404NotFound,
                _ => StatusCodes.Status500InternalServerError
            };

            return new ObjectResult(result)
            {
                StatusCode = statusCode
            };
        }
    }
}
