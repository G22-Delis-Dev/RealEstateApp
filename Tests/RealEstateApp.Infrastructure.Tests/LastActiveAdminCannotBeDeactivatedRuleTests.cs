using RealEstateApp.Domain.Rules.Account;
using Xunit;

namespace RealEstateApp.Infrastructure.Tests;

public class LastActiveAdminCannotBeDeactivatedRuleTests
{
    [Fact]
    public void IsBroken_ShouldReturnTrue_WhenIsLastActiveAdminAndIsBeingDeactivated()
    {
        // Arrange
        int activeAdminCount = 1;
        bool isBeingDeactivated = true;
        var rule = new LastActiveAdminCannotBeDeactivatedRule(activeAdminCount, isBeingDeactivated);

        // Act
        bool isBroken = rule.IsBroken();

        // Assert
        Assert.True(isBroken);
    }

    [Fact]
    public void IsBroken_ShouldReturnFalse_WhenMoreThanOneActiveAdminAndIsBeingDeactivated()
    {
        // Arrange
        int activeAdminCount = 2;
        bool isBeingDeactivated = true;
        var rule = new LastActiveAdminCannotBeDeactivatedRule(activeAdminCount, isBeingDeactivated);

        // Act
        bool isBroken = rule.IsBroken();

        // Assert
        Assert.False(isBroken);
    }

    [Fact]
    public void IsBroken_ShouldReturnFalse_WhenIsNotBeingDeactivated()
    {
        // Arrange
        int activeAdminCount = 1;
        bool isBeingDeactivated = false;
        var rule = new LastActiveAdminCannotBeDeactivatedRule(activeAdminCount, isBeingDeactivated);

        // Act
        bool isBroken = rule.IsBroken();

        // Assert
        Assert.False(isBroken);
    }
}
