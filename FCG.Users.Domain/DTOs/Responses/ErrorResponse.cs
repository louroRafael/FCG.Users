namespace FCG.Users.Domain.DTOs.Responses
{
    public class ErrorResponse
    {
        public string Message { get; set; } = string.Empty;
        public string TraceId { get; set; } = string.Empty;
    }
}
