using CSharpFunctionalExtensions;

namespace DataManagement.Domain.Core;

public class Phone : ValueObject
{
    private Phone(string number)
    {
        Number = number;
    }

    protected Phone()
    {
    }

    public string Number { get; }

    protected override IEnumerable<IComparable> GetEqualityComponents()
    {
        yield return Number;
    }

    public static Result<Phone, Error> Create(Maybe<string> maybeNumber)
    {
        var (_, isFailure, number, error) = ValidateBusinessRules(maybeNumber);
        if (isFailure)
            return error;

        return new Phone(number);
    }

    private static Result<string, Error> ValidateBusinessRules(Maybe<string> number)
    {
        var noNumberProvided =
            number.HasNoValue || string.IsNullOrEmpty(number.Value) || string.IsNullOrWhiteSpace(number.Value);
        if (noNumberProvided)
            return Result.Success<string, Error>(null);

        const int exactLength = 8;

        return Result.Success<string, Error>(number.Value)
            .Ensure(x => x.Length == exactLength,
                DomainErrors.InvalidValue($"{nameof(Phone)} should be {exactLength} characters long"))
            .Map(x => $"{x.Substring(0, 3)}-{x.Substring(3, 3)}-{x.Substring(6, 2)}");
    }
}