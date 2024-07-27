using CSharpFunctionalExtensions;

namespace DataManagement.Domain.Core;

public class Phone : ValueObject
{
    public string Number { get; set; }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Number;
    }
}