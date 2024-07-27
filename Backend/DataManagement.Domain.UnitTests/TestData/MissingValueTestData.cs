using Xunit;

namespace DataManagement.Domain.UnitTests.TestData;

public class MissingValueTestData : TheoryData<string>
{
    public MissingValueTestData()
    {
        Add(null);
        Add(" ");
        Add(string.Empty);
    }
}