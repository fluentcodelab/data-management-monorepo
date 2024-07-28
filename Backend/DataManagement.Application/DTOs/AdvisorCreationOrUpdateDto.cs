using CSharpFunctionalExtensions;

namespace DataManagement.Application.DTOs;

public class AdvisorCreationOrUpdateDto
{
    public Maybe<int> Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string SIN { get; set; }
    public string Address { get; set; }
    public string Phone { get; set; }
}