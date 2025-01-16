using CCL.Security.Identity;
using DAL.Data;
using DAL.Entities;
using DAL.Repositories.Impl.Base;
using DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories.Impl;

public class EmployeeRepository: BaseRepository<Employee>, IEmployeeRepository
{
    private readonly ApplicationDbContext _context;
    public EmployeeRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Employee>> SearchByRole(Role role)
    {
        IQueryable<Employee> query = _context.Employees.Where(e => e.Role == role);
        
        return await query.ToListAsync();
    }

    public async Task<Employee> GetByEmail(string email)
    {
        IQueryable<Employee> query = _context.Employees.Where(e => e.Email == email);
        
        return await query.FirstOrDefaultAsync();
    }
}