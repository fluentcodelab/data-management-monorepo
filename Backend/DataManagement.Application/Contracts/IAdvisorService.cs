using CSharpFunctionalExtensions;
using DataManagement.Application.DTOs;
using DataManagement.Domain;

namespace DataManagement.Application.Contracts;

public interface IAdvisorService
{
    Task<List<AdvisorDto>> GetAsync();
    Task<Result<AdvisorDto, Error>> GetAsync(int id);
    Task<Result<AdvisorDto, List<Error>>> AddAsync(AdvisorCreationDto dto);
    Task<UnitResult<List<Error>>> UpdateAsync(int id, AdvisorUpdateDto dto);
    Task<UnitResult<Error>> DeleteAsync(int id);
}