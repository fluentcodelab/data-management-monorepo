using DataManagement.Application.DTOs;

namespace DataManagement.Application.UnitTests.TestData;

public class AdvisorUpdateDtoBuilder
{
    private readonly AdvisorUpdateDto _dto = new();

    public AdvisorUpdateDtoBuilder WithDefaultData()
    {
        _dto.FirstName = "John";
        _dto.LastName = "Doe";
        _dto.Address = "123; Broadway Street; U0A U5E; ON; CAN";
        _dto.Phone = "12345678";

        return this;
    }

    public AdvisorUpdateDtoBuilder WithFirstName(string firstName)
    {
        _dto.FirstName = firstName;

        return this;
    }

    public AdvisorUpdateDto Build()
    {
        return _dto;
    }
}