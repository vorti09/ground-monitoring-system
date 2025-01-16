using DAL.Data;
using DAL.Entities;
using DAL.Enums;
using DAL.Repositories.Impl.Base;
using DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories.Impl;

public class ReportRepository: BaseRepository<Report>, IReportRepository
{
    private readonly ApplicationDbContext _context;
    public ReportRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }
    
    public async Task<IEnumerable<Report>> GetReportsByEmployeeId(int employeeId)
    {
        var reports = _context.Reports.Where(r => r.EmployeeId == employeeId);
        
        return await reports.ToListAsync();
    }

    public async Task<IEnumerable<Report>> GetByStatus(ReportStatus status)
    {
        var reports = _context.Reports.Where(r => r.Status == status);
        
        return await reports.ToListAsync();
    }
}