using CSharpFunctionalExtensions;

namespace DataManagement.Domain.Core;

public class Advisor : Entity<int>
{
    public FullName FullName { get; set; }
    public SIN SIN { get; set; }
    public Address Address { get; set; }
    public Phone Phone { get; set; }
}