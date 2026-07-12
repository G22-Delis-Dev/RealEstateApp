using RealEstateApp.Domain.Exceptions;

namespace RealEstateApp.Domain.Rules;

public static class BusinessRuleValidator
{
    public static void CheckRule(IBusinessRule rule)
    {
        if (rule.IsBroken())
            throw new BusinessRuleValidationException(rule);
    }

    public static void CheckRules(params IBusinessRule[] rules)
    {
        foreach (var rule in rules)
            CheckRule(rule);
    }
}