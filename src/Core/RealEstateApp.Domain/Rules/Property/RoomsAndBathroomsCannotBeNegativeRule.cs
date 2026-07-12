namespace RealEstateApp.Domain.Rules.Property;

public sealed class RoomsAndBathroomsCannotBeNegativeRule : IBusinessRule
{
    private readonly int _rooms;
    private readonly int _bathrooms;

    public RoomsAndBathroomsCannotBeNegativeRule(int rooms, int bathrooms)
    {
        _rooms = rooms;
        _bathrooms = bathrooms;
    }

    public bool IsBroken() => _rooms < 0 || _bathrooms < 0;
    public string Message => "La cantidad de habitaciones y baños no puede ser menor que cero.";
}