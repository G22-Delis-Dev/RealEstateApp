namespace RealEstateApp.Domain.Rules.Account;

public sealed class UsernameMustBeUniqueRule : IBusinessRule
{
    private readonly bool _usernameAlreadyExists;
    public UsernameMustBeUniqueRule(bool usernameAlreadyExists) => _usernameAlreadyExists = usernameAlreadyExists;

    public bool IsBroken() => _usernameAlreadyExists;
    public string Message => "Ya existe un usuario registrado con este nombre de usuario.";
}
