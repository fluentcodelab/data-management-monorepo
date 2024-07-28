using CSharpFunctionalExtensions;
using DataManagement.Domain.Core;

namespace DataManagement.Application.DTOs;

public static class Mappings
{
    public static AdvisorDto ToDto(this Advisor advisor)
    {
        return new AdvisorDto
        {
            Id = advisor.Id,
            FirstName = advisor.FullName.FirstName,
            LastName = advisor.FullName.LastName,
            SIN = advisor.SIN.Number,
            Address = advisor.Address.Value,
            Phone = advisor.Phone == Maybe<Phone>.None ? null : advisor.Phone.Number,
            HealthStatus = advisor.HealthStatus.ToString()
        };
    }

    public static List<AdvisorDto> ToDto(this List<Advisor> advisors)
    {
        return advisors.Select(x => x.ToDto()).ToList();
    }
}