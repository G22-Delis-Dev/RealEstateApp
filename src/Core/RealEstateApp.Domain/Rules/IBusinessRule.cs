namespace RealEstateApp.Domain.Rules;

public interface IBusinessRule
{
    bool IsBroken();
    string Message { get; }
}