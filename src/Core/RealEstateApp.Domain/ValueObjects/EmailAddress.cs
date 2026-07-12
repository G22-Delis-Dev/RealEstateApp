using System.Collections.Generic;
using RealEstateApp.Domain.Common;

namespace RealEstateApp.Domain.ValueObjects;

public class EmailAddress : ValueObject
{
    public string Address { get; private set; }

    private EmailAddress() { Address = null!; }

    public EmailAddress(string address)
    {
        if (string.IsNullOrWhiteSpace(address) || !address.Contains("@"))
            throw new ArgumentException("El correo electrónico no es válido.", nameof(address));

        Address = address;
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Address;
    }
}
