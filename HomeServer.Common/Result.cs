namespace HomeServer.Common;

public class Result<TEntity>
{
    public bool IsSuccess { get; }

    public bool IsFailure => !IsSuccess;
    
    public Error Error { get; }
    
    private readonly TEntity? _value;
    
    public TEntity Value
    {
        get
        {
            if (IsFailure)
            {
                throw new InvalidOperationException("Result object does not have a value.");
            }

            return _value!;
        }
    }

    private Result(TEntity value)
    {
        IsSuccess = true;
        _value = value;
        Error = Error.None;
    }
    
    private Result(Error error)
    {
        if (error == Error.None)
        {
            throw new ArgumentException("Error object must not be Error.None", nameof(error));
        }
        
        IsSuccess = false;
        Error = error;
    }
    
    public static Result<TEntity> Success(TEntity value)
    {
        return new Result<TEntity>(value);
    }
    public static Result<TEntity> Failure(Error error)
    {
        return new Result<TEntity>(error);
    }
}