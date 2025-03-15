namespace HomeServer.Common;

public record Error(string Code, string Message)
{
    public static readonly Error None = new Error(string.Empty, string.Empty);

    private const string RecordNotFoundCode = $"{nameof(Error)}.{nameof(RecordNotFoundCode)}";
    private const string ValidationErrorCode = $"{nameof(Error)}.{nameof(ValidationErrorCode)}";
    private const string EntityAlreadyExistsCode = $"{nameof(Error)}.{nameof(EntityAlreadyExistsCode)}";
    
    public static Error RecordNotFound(string message) => new(RecordNotFoundCode, message);
    public static Error ValidationError(string message) => new(ValidationErrorCode, message);
    public static Error EntityAlreadyExists(string message) => new(EntityAlreadyExistsCode, message);
}