namespace RealEstateApp.Domain.Rules.Account;

public sealed class UserMustBeActiveRule : IBusinessRule
{
    private readonly bool _isActive;
    public UserMustBeActiveRule(bool isActive) => _isActive = isActive;

    public bool IsBroken() => !_isActive;
    public string Message => "El usuario se encuentra inactivo y no puede iniciar sesión.";
}
