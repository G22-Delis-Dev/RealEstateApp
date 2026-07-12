namespace RealEstateApp.Domain.Rules.Account;

public sealed class EmailMustBeUniqueRule : IBusinessRule
{
    private readonly bool _emailAlreadyExists;
    public EmailMustBeUniqueRule(bool emailAlreadyExists) => _emailAlreadyExists = emailAlreadyExists;

    public bool IsBroken() => _emailAlreadyExists;
    public string Message => "Ya existe un usuario registrado con este correo electrónico.";
}
