// Application/Common/Exceptions/NotFoundException.cs
namespace RealEstateApp.Application.Common.Exceptions;

public class NotFoundException : Exception
{
    public NotFoundException(string entityName, object key)
        : base($"No se encontró {entityName} con identificador '{key}'.") { }
}