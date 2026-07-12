namespace RealEstateApp.Domain.Rules.Account;

public sealed class PasswordsMustMatchRule : IBusinessRule
{
    private readonly string _password;
    private readonly string _confirmPassword;

    public PasswordsMustMatchRule(string password, string confirmPassword)
    {
        _password = password;
        _confirmPassword = confirmPassword;
    }

    public bool IsBroken() => _password != _confirmPassword;
    public string Message => "Las contraseñas no coinciden.";
}
