using DataManagement.Application.DTOs;

namespace DataManagement.Application.UnitTests.TestData;

public class AdvisorCreationDtoBuilder
{
    private readonly AdvisorCreationDto _dto = new();

    public AdvisorCreationDtoBuilder WithDefaultData()
    {
        _dto.FirstName = "John";
        _dto.LastName = "Doe";
        _dto.Address = "123; Broadway Street; U0A U5E; ON; CAN";
        _dto.Phone = "12345678";
        _dto.SIN = "123456789";

        return this;
    }

    public AdvisorCreationDtoBuilder WithFirstName(string firstName)
    {
        _dto.FirstName = firstName;

        return this;
    }

    public AdvisorCreationDto Build()
    {
        return _dto;
    }
}