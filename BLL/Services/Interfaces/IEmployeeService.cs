using BLL.DTOs;
using CCL.Security.Identity;

namespace BLL.Services.Interfaces;

public interface IEmployeeService
{
    Task CreateAsync(EmployeeDto employeeDto);
    Task UpdateAsync(EmployeeDto employeeDto);
    Task DeleteAsync(int employeeId);
    Task<EmployeeDto> GetByIdAsync(int employeeId);
    Task<List<EmployeeDto>> GetAllAsync();
    IEnumerable<EmployeeDto> SearchByRole(Role role);
    Task<EmployeeDto> GetByEmailAsync(string email);
    Task AssignRoleAsync(int employeeId, Role role);
    IEnumerable<EmployeeDto> GetEmployeesFiltered(int pageNumber);
}