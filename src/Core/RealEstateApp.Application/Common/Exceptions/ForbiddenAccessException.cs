// Application/Common/Exceptions/ForbiddenAccessException.cs
namespace RealEstateApp.Application.Common.Exceptions;

public class ForbiddenAccessException : Exception
{
    public ForbiddenAccessException(string message = "No tiene permisos para realizar esta acción.")
        : base(message) { }
}