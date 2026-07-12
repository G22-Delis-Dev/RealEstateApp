using System;
using System.Collections.Generic;

namespace RealEstateApp.Application.Common.Exceptions;

// Excepción para cuando fallan validaciones de los datos de entrada
public class ValidationException : Exception
{
    public IReadOnlyDictionary<string, string[]> Errors { get; }

    public ValidationException()
        : base("Han ocurrido uno o más errores de validación.")
    {
        Errors = new Dictionary<string, string[]>();
    }

    public ValidationException(IReadOnlyDictionary<string, string[]> errors)
        : base("Han ocurrido uno o más errores de validación.")
    {
        Errors = errors;
    }
}
