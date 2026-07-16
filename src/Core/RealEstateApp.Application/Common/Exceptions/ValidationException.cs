// Application/Common/Exceptions/ValidationException.cs
namespace RealEstateApp.Application.Common.Exceptions;

public class ValidationException : Exception
{
    public ValidationException(string message) : base(message) { }
}