using DAL.Entities;
using DAL.Enums;

namespace DAL.Repositories.Interfaces;

public interface IReportRepository : IRepository<Report>
{
    Task<IEnumerable<Report>> GetReportsByEmployeeId(int employeeId);
    Task<IEnumerable<Report>> GetByStatus(ReportStatus status);
}