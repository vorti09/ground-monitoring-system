using CCL.Security.Identity;
using DAL.Entities;

namespace DAL.Repositories.Interfaces;

public interface IEmployeeRepository : IRepository<Employee>
{
    Task<IEnumerable<Employee>> SearchByRole(Role role);
    Task<Employee> GetByEmail(string email);
}