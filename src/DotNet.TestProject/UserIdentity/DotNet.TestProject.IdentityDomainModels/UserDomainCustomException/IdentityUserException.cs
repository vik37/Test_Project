namespace DotNet.TestProject.IdentityDomainModels.UserDomainCustomException;

public class IdentityUserException : Exception
{
    public IdentityUserException()
    {}

    public IdentityUserException(string message)
        : base(message) { }

    public IdentityUserException(string message, Exception innerException)
        : base(message,innerException) { }
}