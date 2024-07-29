using System.Linq.Expressions;
using CSharpFunctionalExtensions;
using DataManagement.Application.Contracts;
using DataManagement.Domain.Core;
using Microsoft.EntityFrameworkCore;

namespace DataManagement.Infrastructure;

public class AdvisorRepository(DataManagementDbContext dbContext) : IAdvisorRepository
{
    private readonly DbSet<Advisor> _dbSet = dbContext.Set<Advisor>();

    public async Task<List<Advisor>> GetAsync(Expression<Func<Advisor, bool>> filter = null)
    {
        IQueryable<Advisor> query = _dbSet;

        if (filter != null)
            query = query.Where(filter);

        return await query.ToListAsync();
    }

    public async Task<Maybe<Advisor>> GetAsync(int id)
    {
        return await _dbSet.FindAsync(id);
    }

    public async Task AddAsync(Advisor advisor)
    {
        await _dbSet.AddAsync(advisor);
    }

    public void Update(Advisor advisor)
    {
        _dbSet.Attach(advisor);
        dbContext.Entry(advisor).State = EntityState.Modified;
    }

    public void Delete(Advisor advisor)
    {
        if (dbContext.Entry(advisor).State == EntityState.Detached)
            _dbSet.Attach(advisor);
        _dbSet.Remove(advisor);
    }

    public async Task SaveAsync()
    {
        await dbContext.SaveChangesAsync();
    }
}