using RealEstateApp.Domain.Rules;

namespace RealEstateApp.Domain.Exceptions;

public sealed class BusinessRuleValidationException : DomainException
{
    public IBusinessRule BrokenRule { get; }

    public BusinessRuleValidationException(IBusinessRule brokenRule)
        : base(brokenRule.Message)
    {
        BrokenRule = brokenRule;
    }
}
