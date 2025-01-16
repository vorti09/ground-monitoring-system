namespace DAL.Repositories.Interfaces;

public interface IRepository<T> where T : class
{
    Task<IEnumerable<T>> GetAll();
    Task<T?> GetById(int id);
    IEnumerable<T> Find(Func<T, bool> predicate, int pageNumber, int pageSize);
    Task Create(T entity);
    void Update(T entity);
    Task Delete(int id);
}