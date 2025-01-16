using DAL.Repositories.Interfaces;

namespace DAL.UnitOfWork;

public interface IUnitOfWork : IDisposable
{
    public IRepository<TEntity> GetRepository<TEntity>(bool hasCustomRepository = false) where TEntity : class;
    Task SaveChangesAsync();
    IEmployeeRepository Employees { get; }
    IIndicatorRepository Indicators { get;  }
    IReportRepository Reports { get; }
}