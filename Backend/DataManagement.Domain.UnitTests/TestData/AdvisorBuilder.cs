using Bogus;
using CSharpFunctionalExtensions;
using DataManagement.Domain.Core;

namespace DataManagement.Domain.UnitTests.TestData;

public class AdvisorBuilder
{
    private readonly AdvisorTestModel _model = new();

    public AdvisorBuilder WithDefaultData()
    {
        var person = new Person();

        _model.FullName = FullName.Create(person.FirstName, person.LastName).Value;
        _model.SIN = SIN.Create("123456789").Value;
        _model.Phone = Phone.Create("12345678").Value;
        _model.Address = Address.Create("123, Broadway Street, U0A U5E, ON, CAN").Value;

        return this;
    }

    public AdvisorBuilder WithFullName(Maybe<FullName> maybeFullName)
    {
        _model.FullName = maybeFullName.HasValue ? maybeFullName.Value : null;
        return this;
    }

    public AdvisorBuilder WithSIN(Maybe<SIN> maybeSIN)
    {
        _model.SIN = maybeSIN.HasValue ? maybeSIN.Value : null;
        return this;
    }

    public AdvisorBuilder WithAddress(Maybe<Address> maybeAddress)
    {
        _model.Address = maybeAddress.HasValue ? maybeAddress.Value : null;
        return this;
    }

    public AdvisorBuilder WithPhone(Maybe<Phone> maybePhone)
    {
        _model.Phone = maybePhone.HasValue ? maybePhone.Value : null;
        return this;
    }

    public Result<Advisor, Error> Build()
    {
        return Advisor.Create(_model.FullName, _model.SIN, _model.Address, _model.Phone);
    }

    private class AdvisorTestModel
    {
        public FullName FullName { get; set; }
        public SIN SIN { get; set; }
        public Address Address { get; set; }
        public Phone Phone { get; set; }
    }
}