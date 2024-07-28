using System.Linq.Expressions;
using DataManagement.Domain;
using DataManagement.Domain.Core;

namespace DataManagement.Application.DTOs;

public class AdvisorValidationDto
{
    public FullName FullName { get; set; }
    public SIN SIN { get; set; }
    public Address Address { get; set; }
    public Phone Phone { get; set; }
    public List<Error> Errors { get; set; } = new();
    public Expression<Func<Advisor, bool>> AdvisorWithSameInfo { get; set; }
}