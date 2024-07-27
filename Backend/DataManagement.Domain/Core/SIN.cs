using CSharpFunctionalExtensions;

namespace DataManagement.Domain.Core;

public class SIN : ValueObject
{
    public string Number { get; set; }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Number;
    }
}