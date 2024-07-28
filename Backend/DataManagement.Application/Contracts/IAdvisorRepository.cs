using System.Linq.Expressions;
using CSharpFunctionalExtensions;
using DataManagement.Domain.Core;

namespace DataManagement.Application.Contracts;

public interface IAdvisorRepository
{
    Task<List<Advisor>> GetAsync(Expression<Func<Advisor, bool>> filter = null);
    Task<Maybe<Advisor>> GetAsync(int id);
    Task AddAsync(Advisor advisor);
    void Update(Advisor advisor);
    void Delete(Advisor advisor);
    Task SaveAsync();
}