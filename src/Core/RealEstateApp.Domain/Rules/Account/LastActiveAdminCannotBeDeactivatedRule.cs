namespace RealEstateApp.Domain.Rules.Account;

public sealed class LastActiveAdminCannotBeDeactivatedRule : IBusinessRule
{
    private readonly int _activeAdminCount;
    private readonly bool _isBeingDeactivated;

    public LastActiveAdminCannotBeDeactivatedRule(int activeAdminCount, bool isBeingDeactivated)
    {
        _activeAdminCount = activeAdminCount;
        _isBeingDeactivated = isBeingDeactivated;
    }

    public bool IsBroken() => _isBeingDeactivated && _activeAdminCount <= 1;
    public string Message => "No se puede desactivar al último administrador activo del sistema.";
}
