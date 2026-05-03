namespace CarStock.Helpers.Exceptions;

public class AddException : Exception
{
    public string ErrorCode { get; }

    public AddException(string errorCode, string message) : base(message)
    {
        ErrorCode = errorCode;
    }
}