using DataManagement.Domain.Core;
using DataManagement.Domain.UnitTests.TestData;
using Shouldly;
using Xunit;

namespace DataManagement.Domain.UnitTests;

public class SINShould
{
    [Theory(DisplayName = "Fail on creation, if sin is missing")]
    [ClassData(typeof(MissingValueTestData))]
    public void Fail_OnCreation_MissingFirstName(string missing)
    {
        var result = SIN.Create(missing);

        result.IsFailure.ShouldBeTrue();
        result.Error.Message.ShouldBe("Value for field SIN is missing");
    }

    [Fact(DisplayName = "Fail on creation, if SIN is not 9 characters long")]
    public void Fail_OnCreation_FirstNameTooLong()
    {
        var result = SIN.Create("12345678");

        result.IsFailure.ShouldBeTrue();
        result.Error.Message.ShouldBe("SIN should be 9 characters long");
    }

    [Fact(DisplayName = "Succeed on creation, if sin is valid")]
    public void Succeed_OnCreation_Valid()
    {
        var result = SIN.Create("123456789");

        result.IsSuccess.ShouldBeTrue();
        result.Value.Number.ShouldBe("123-456-789");
    }
}