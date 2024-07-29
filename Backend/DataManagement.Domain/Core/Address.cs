using CSharpFunctionalExtensions;

namespace DataManagement.Domain.Core;

public class Address : ValueObject
{
    protected Address()
    {
    }

    private Address(string value)
    {
        Value = value;
    }

    public string Value { get; }

    protected override IEnumerable<IComparable> GetEqualityComponents()
    {
        yield return Value;
    }

    public static Result<Address, Error> Create(Maybe<string> maybeAddress)
    {
        var (_, isFailure, address, error) = ValidateBusinessRules(maybeAddress);
        if (isFailure)
            return error;

        return new Address(address);
    }

    private static Result<string, Error> ValidateBusinessRules(Maybe<string> address)
    {
        var noAddressProvided =
            address.HasNoValue || string.IsNullOrEmpty(address.Value) || string.IsNullOrWhiteSpace(address.Value);
        if (noAddressProvided)
            return Result.Success<string, Error>(null);

        const int fieldMaxLength = 255;

        return Result.Success<string, Error>(address.Value)
            .Ensure(x => x.Length <= fieldMaxLength,
                DomainErrors.InvalidValue($"{nameof(Address)} should not exceed {fieldMaxLength} characters"));
    }
}