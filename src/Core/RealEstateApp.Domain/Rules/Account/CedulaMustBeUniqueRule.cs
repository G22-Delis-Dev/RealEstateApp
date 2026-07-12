namespace RealEstateApp.Domain.Rules.Account;

public sealed class CedulaMustBeUniqueRule : IBusinessRule
{
    private readonly bool _cedulaAlreadyExists;
    public CedulaMustBeUniqueRule(bool cedulaAlreadyExists) => _cedulaAlreadyExists = cedulaAlreadyExists;

    public bool IsBroken() => _cedulaAlreadyExists;
    public string Message => "Ya existe un usuario registrado con esta cédula.";
}
