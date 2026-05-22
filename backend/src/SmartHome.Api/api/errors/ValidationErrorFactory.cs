namespace SmartHome.Api.Api.Errors;

public sealed record ValidationError(string Code, string Message, string? Field, string RequestId);

public sealed class ValidationErrorFactory
{
    public ValidationError Create(string code, string message, HttpContext context, string? field = null)
    {
        return new ValidationError(code, message, field, context.TraceIdentifier);
    }
}
