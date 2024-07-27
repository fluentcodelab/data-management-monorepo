using CSharpFunctionalExtensions;

namespace DataManagement.Domain.Core;

public class Address : ValueObject
{
    public string Street { get; set; }
    public string ZipCode { get; set; }
    public string Province { get; set; }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Street;
        yield return ZipCode;
        yield return Province;
    }
}