using FCG.Users.Domain.DTOs.Responses;
using FCG.Users.Domain.Interfaces.Common;

namespace FCG.Users.API.Middlewares
{
    public class ExceptionMiddleware(IAppLogger<ExceptionMiddleware> logger, IHostEnvironment env) : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                var traceId = context.TraceIdentifier;

                logger.LogError(
                    "Unhandled exception. TraceId: {TraceId}",
                    ex,
                    traceId
                );

                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                context.Response.ContentType = "application/json";

                var errorResponse = new ErrorResponse
                {
                    Message = env.IsDevelopment()
                        ? ex.Message
                        : "Ocorreu um erro inesperado.",
                    TraceId = traceId
                };

                await context.Response.WriteAsJsonAsync(errorResponse);
            }
        }
    }
}
