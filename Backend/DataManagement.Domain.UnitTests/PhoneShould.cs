using DataManagement.Domain.Core;
using DataManagement.Domain.UnitTests.TestData;
using Shouldly;
using Xunit;

namespace DataManagement.Domain.UnitTests;

public class PhoneShould
{
    [Theory(DisplayName = "Succeed on creation, if phone number is missing")]
    [ClassData(typeof(MissingValueTestData))]
    public void Fail_OnCreation_MissingNumber(string missing)
    {
        var result = Phone.Create(missing);

        result.IsSuccess.ShouldBeTrue();
        result.Value.Number.ShouldBeNull();
    }

    [Fact(DisplayName = "Fail on creation, if Phone is not 8 characters long")]
    public void Fail_OnCreation_NumberNot8CharsLong()
    {
        var result = Phone.Create("1234567");

        result.IsFailure.ShouldBeTrue();
        result.Error.Message.ShouldBe("Phone should be 8 characters long");
    }

    [Fact(DisplayName = "Succeed on creation, if phone is valid")]
    public void Succeed_OnCreation_Valid()
    {
        var result = Phone.Create("12345678");

        result.IsSuccess.ShouldBeTrue();
        result.Value.Number.ShouldBe("123-456-78");
    }
}