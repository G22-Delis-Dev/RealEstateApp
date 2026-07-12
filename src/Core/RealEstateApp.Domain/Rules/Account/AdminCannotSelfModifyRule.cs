namespace RealEstateApp.Domain.Rules.Account;

public sealed class AdminCannotSelfModifyRule : IBusinessRule
{
    private readonly string _currentUserId;
    private readonly string _targetUserId;

    public AdminCannotSelfModifyRule(string currentUserId, string targetUserId)
    {
        _currentUserId = currentUserId;
        _targetUserId = targetUserId;
    }

    public bool IsBroken() => _currentUserId == _targetUserId;
    public string Message => "Un administrador no puede modificar su propia cuenta desde este módulo.";
}
