using CSharpFunctionalExtensions;
using DataManagement.Domain.Core;
using DataManagement.Domain.UnitTests.TestData;
using Shouldly;
using Xunit;

namespace DataManagement.Domain.UnitTests;

public class AdvisorShould
{
    [Fact(DisplayName = "Fail on creation, if name is missing")]
    public void Fail_OnCreation_MissingName()
    {
        Assert.Throws<ArgumentNullException>(() =>
        {
            new AdvisorBuilder()
                .WithDefaultData()
                .WithFullName(Maybe<FullName>.None)
                .Build();
        });
    }

    [Fact(DisplayName = "Fail on creation, if SIN is missing")]
    public void Fail_OnCreation_MissingSIN()
    {
        Assert.Throws<ArgumentNullException>(() =>
        {
            new AdvisorBuilder()
                .WithDefaultData()
                .WithSIN(Maybe<SIN>.None)
                .Build();
        });
    }

    [Fact(DisplayName = "Succeed on creation, if Address is missing")]
    public void Succeed_OnCreation_MissingAddress()
    {
        var result = new AdvisorBuilder()
            .WithDefaultData()
            .WithAddress(Maybe<Address>.None)
            .Build();

        result.IsSuccess.ShouldBeTrue();
        result.Value.Address.ShouldBeNull();
    }

    [Fact(DisplayName = "Succeed on creation, if Phone is missing")]
    public void Succeed_OnCreation_MissingPhone()
    {
        var result = new AdvisorBuilder()
            .WithDefaultData()
            .WithPhone(Maybe<Phone>.None)
            .Build();

        result.IsSuccess.ShouldBeTrue();
        result.Value.Phone.ShouldBeNull();
    }

    [Fact(DisplayName = "Succeed on creation, if valid data is provided")]
    public void Succeed_OnCreation_ValidData()
    {
        var result = new AdvisorBuilder()
            .WithDefaultData()
            .Build();

        result.IsSuccess.ShouldBeTrue();
    }
}