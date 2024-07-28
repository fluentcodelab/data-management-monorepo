using CSharpFunctionalExtensions;
using DataManagement.Application.DTOs;

namespace DataManagement.Application.UnitTests.TestData;

public class AdvisorCreationOrUpdateDtoBuilder
{
    private readonly AdvisorCreationOrUpdateDto _dto = new();

    public AdvisorCreationOrUpdateDtoBuilder WithDefaultData()
    {
        _dto.Id = 1;
        _dto.FirstName = "John";
        _dto.LastName = "Doe";
        _dto.Address = "123; Broadway Street; U0A U5E; ON; CAN";
        _dto.Phone = "12345678";
        _dto.SIN = "123456789";

        return this;
    }

    public AdvisorCreationOrUpdateDtoBuilder WithId(Maybe<int> id)
    {
        _dto.Id = id;

        return this;
    }

    public AdvisorCreationOrUpdateDtoBuilder WithFirstName(string firstName)
    {
        _dto.FirstName = firstName;

        return this;
    }

    public AdvisorCreationOrUpdateDto Build()
    {
        return _dto;
    }
}