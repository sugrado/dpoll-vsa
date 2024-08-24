namespace Api.Shared.Exceptions;

public class AuthorizationException : Exception
{
    public AuthorizationException() { }

    public AuthorizationException(string? message)
        : base(message) { }

    public AuthorizationException(string? message, Exception? innerException)
        : base(message, innerException) { }
}
