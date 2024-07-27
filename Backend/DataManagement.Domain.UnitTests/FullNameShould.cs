using Bogus;
using DataManagement.Domain.Core;
using DataManagement.Domain.UnitTests.TestData;
using Shouldly;
using Xunit;

namespace DataManagement.Domain.UnitTests;

public class FullNameShould
{
    [Theory(DisplayName = "Fail on creation, if first name is missing")]
    [ClassData(typeof(MissingValueTestData))]
    public void Fail_OnCreation_MissingFirstName(string missing)
    {
        var result = FullName.Create(missing, "Doe");

        result.IsFailure.ShouldBeTrue();
        result.Error.Any(x => x.Message.Equals("Value for field FirstName is missing")).ShouldBeTrue();
    }

    [Fact(DisplayName = "Fail on creation, if first name contains more than 255 characters")]
    public void Fail_OnCreation_FirstNameTooLong()
    {
        var firstName = new Faker().Random.String2(256);
        var result = FullName.Create(firstName, "Doe");

        result.IsFailure.ShouldBeTrue();
        result.Error.Any(x => x.Message.Equals("FirstName should not exceed 255 characters")).ShouldBeTrue();
    }

    [Theory(DisplayName = "Fail on creation, if last name is missing")]
    [ClassData(typeof(MissingValueTestData))]
    public void Fail_OnCreation_MissingLastName(string missing)
    {
        var result = FullName.Create("John", missing);

        result.IsFailure.ShouldBeTrue();
        result.Error.Any(x => x.Message.Equals("Value for field LastName is missing")).ShouldBeTrue();
    }

    [Fact(DisplayName = "Fail on creation, if last name contains more than 255 characters")]
    public void Fail_OnCreation_LastNameTooLong()
    {
        var lastName = new Faker().Random.String2(256);
        var result = FullName.Create("John", lastName);

        result.IsFailure.ShouldBeTrue();
        result.Error.Any(x => x.Message.Equals("LastName should not exceed 255 characters")).ShouldBeTrue();
    }

    [Fact(DisplayName = "Succeed on creation, if last name and first name are valid")]
    public void Succeed_OnCreation_Valid()
    {
        var result = FullName.Create("John", "Doe");

        result.IsSuccess.ShouldBe(true);
        result.Value.ToString().ShouldBe("Doe, John");
    }
}