using CSharpFunctionalExtensions;

namespace DataManagement.Domain.Core;

public class FullName : ValueObject
{
    private FullName(string firstName, string lastName)
    {
        FirstName = firstName;
        LastName = lastName;
    }

    protected FullName()
    {
    }

    public string FirstName { get; }
    public string LastName { get; }

    protected override IEnumerable<IComparable> GetEqualityComponents()
    {
        yield return FirstName;
        yield return LastName;
    }

    public override string ToString()
    {
        return $"{LastName}, {FirstName}";
    }

    public static Result<FullName, List<Error>> Create(string firstName, string lastName)
    {
        var errors = new List<Error>();
        var firstNameValidation = ValidateBusinessRules(firstName, nameof(FirstName));
        var lastNameValidation = ValidateBusinessRules(lastName, nameof(LastName));

        if (firstNameValidation.IsFailure)
            errors.Add(firstNameValidation.Error);
        if (lastNameValidation.IsFailure)
            errors.Add(lastNameValidation.Error);

        if (errors.Count > 0) return errors;

        return new FullName(firstName, lastName);
    }

    private static Result<string, Error> ValidateBusinessRules(Maybe<string> name, string fieldName)
    {
        const int fieldMaxLength = 255;
        var missingFieldMessage = $"Value for field {fieldName} is missing";
        var fieldTooLongMessage = $"{fieldName} should not exceed {fieldMaxLength} characters";
        return name
            .ToResult(DomainErrors.InvalidValue(missingFieldMessage))
            .Map(x => x.Trim())
            .Ensure(x => x != string.Empty, DomainErrors.InvalidValue(missingFieldMessage))
            .Ensure(x => x.Length <= fieldMaxLength, DomainErrors.InvalidValue(fieldTooLongMessage));
    }
}