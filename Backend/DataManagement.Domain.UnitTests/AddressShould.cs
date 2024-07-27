using Bogus;
using DataManagement.Domain.Core;
using DataManagement.Domain.UnitTests.TestData;
using Shouldly;
using Xunit;

namespace DataManagement.Domain.UnitTests;

public class AddressShould
{
    [Theory(DisplayName = "Succeed on creation, if address is missing")]
    [ClassData(typeof(MissingValueTestData))]
    public void Fail_OnCreation_MissingAddress(string missing)
    {
        var result = Address.Create(missing);

        result.IsSuccess.ShouldBeTrue();
        result.Value.Value.ShouldBeNull();
    }

    [Fact(DisplayName = "Fail on creation, if Address exceeds 255 characters")]
    public void Fail_OnCreation_AddressTooLong()
    {
        var result = Address.Create(new Faker().Random.String2(256));

        result.IsFailure.ShouldBeTrue();
        result.Error.Message.ShouldBe("Address should not exceed 255 characters");
    }

    [Fact(DisplayName = "Succeed on creation, if phone is valid")]
    public void Succeed_OnCreation_Valid()
    {
        const string address = "123, Broadway Street, U0A U5E, ON, CAN";
        var result = Address.Create(address);

        result.IsSuccess.ShouldBeTrue();
        result.Value.Value.ShouldBe(address);
    }
}