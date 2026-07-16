// Application/Common/Exceptions/ConflictException.cs
namespace RealEstateApp.Application.Common.Exceptions;

public class ConflictException : Exception
{
    public ConflictException(string message) : base(message) { }
}