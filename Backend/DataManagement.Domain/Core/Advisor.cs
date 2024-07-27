using CSharpFunctionalExtensions;

namespace DataManagement.Domain.Core;

public class Advisor : Entity<int>
{
    private Advisor(FullName fullName, SIN sin, Maybe<Address> maybeAddress, Maybe<Phone> maybePhone)
    {
        FullName = fullName;
        SIN = sin;
        Address = maybeAddress.HasValue ? maybeAddress.Value : null;
        Phone = maybePhone.HasValue ? maybePhone.Value : null;
        HealthStatus = GenerateRandomHealthStatus();
    }

    public FullName FullName { get; }
    public SIN SIN { get; }
    public Address Address { get; }
    public Phone Phone { get; }
    public HealthStatus HealthStatus { get; }

    public static Result<Advisor, Error> Create(Maybe<FullName> maybeFullName, Maybe<SIN> maybeSin,
        Maybe<Address> maybeAddress, Maybe<Phone> maybePhone)
    {
        var fullName = maybeFullName.GetValueOrThrow(new ArgumentNullException());
        var sin = maybeSin.GetValueOrThrow(new ArgumentNullException());

        return new Advisor(fullName, sin, maybeAddress, maybePhone);
    }

    private static HealthStatus GenerateRandomHealthStatus()
    {
        var random = new Random();
        var randomValue = random.Next(1, 101);

        return randomValue switch
        {
            <= 60 => HealthStatus.Green,
            <= 80 => HealthStatus.Yellow,
            _ => HealthStatus.Red
        };
    }
}