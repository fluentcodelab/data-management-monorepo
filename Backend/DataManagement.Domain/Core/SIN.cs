using CSharpFunctionalExtensions;

namespace DataManagement.Domain.Core;

public class SIN : ValueObject
{
    private SIN(string number)
    {
        Number = number;
    }

    protected SIN()
    {
    }

    public string Number { get; }

    protected override IEnumerable<IComparable> GetEqualityComponents()
    {
        yield return Number;
    }

    public static Result<SIN, Error> Create(Maybe<string> sin)
    {
        var (_, isFailure, value, error) = ValidateBusinessRules(sin);
        if (isFailure)
            return error;

        return new SIN(value);
    }

    private static Result<string, Error> ValidateBusinessRules(Maybe<string> name)
    {
        const int exactLength = 9;
        const string missingFieldMessage = $"Value for field {nameof(SIN)} is missing";
        var wrongLengthMessage = $"{nameof(SIN)} should be {exactLength} characters long";

        return name
            .ToResult(DomainErrors.InvalidValue(missingFieldMessage))
            .Map(x => x.Trim())
            .Ensure(x => x != string.Empty, DomainErrors.InvalidValue(missingFieldMessage))
            .Ensure(x => x.Length == exactLength, DomainErrors.InvalidValue(wrongLengthMessage))
            .Map(x => $"{x.Substring(0, 3)}-{x.Substring(3, 3)}-{x.Substring(6, 3)}");
    }
}