using DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories.Impl.Base;

public class BaseRepository<T> : IRepository<T> where T : class
{
    private readonly DbSet<T> _set;

    public BaseRepository(DbContext context)
    {
        _set = context.Set<T>();
    }

    public async Task Create(T entity)
    {
        await _set.AddAsync(entity);
    }

    public async Task<T?> GetById(int id)
    {
        return await _set.FindAsync(id);
    }

    public async Task Delete(int id)
    {
        var entity = await GetById(id);
        _set.Remove(entity);
    }

    public async Task<IEnumerable<T>> GetAll()
    {
        return await _set.ToListAsync();
    }

    public void Update(T entity)
    {
        _set.Update(entity);
    }

    public IEnumerable<T> Find(
        Func<T, bool> predicate,
        int pageNumber = 0,
        int pageSize = 10)
    {
        return _set.Where(predicate)
            .Skip(pageSize * pageNumber)
            .Take(pageNumber)
            .ToList();
    }
}