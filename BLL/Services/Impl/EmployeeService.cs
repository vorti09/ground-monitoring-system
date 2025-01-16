using AutoMapper;
using BLL.DTOs;
using BLL.Exceptions;
using BLL.Services.Interfaces;
using CCL.Security;
using CCL.Security.Identity;
using DAL.Entities;
using DAL.Repositories.Interfaces;
using DAL.UnitOfWork;

namespace BLL.Services.Impl;

public class EmployeeService : IEmployeeService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IMapper _mapper;
    private int pageSize = 10;

    public EmployeeService(
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        ArgumentNullException.ThrowIfNull(unitOfWork);
        _employeeRepository = unitOfWork.Employees;
        _mapper = mapper;
    }

    public async Task CreateAsync(EmployeeDto employeeDto)
    {
        var employee = await _employeeRepository.GetById(employeeDto.Id);
        if (employee is not null)
        {
            throw new EntityAlreadyExistsException();
        }
        
        await _employeeRepository.Create(_mapper.Map<Employee>(employeeDto));
        await _unitOfWork.SaveChangesAsync();
    }
    
    public async Task UpdateAsync(EmployeeDto employeeDto)
    {
        var employee = await _employeeRepository.GetById(employeeDto.Id);
        if (employee is null)
        {
            throw new EntityNotFoundException();
        }
        
        var employeeEntity = _mapper.Map<Employee>(employeeDto);
        
        _employeeRepository.Update(employeeEntity);
        await _unitOfWork.SaveChangesAsync();
    }
    
    public async Task DeleteAsync(int employeeId)
    {
        var employee = await _employeeRepository.GetById(employeeId);
        if (employee is null)
        {
            throw new EntityNotFoundException();
        }
        
        await _employeeRepository.Delete(employee.Id);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<EmployeeDto> GetByIdAsync(int employeeId)
    {
        var employee = await _employeeRepository.GetById(employeeId);
        if (employee is null)
        {
            throw new EntityNotFoundException();
        }
        
        var employeeDto = _mapper.Map<EmployeeDto>(employee);
        
        return employeeDto;
    }
    
    public async Task<List<EmployeeDto>> GetAllAsync()
    {
        var employees = await _employeeRepository.GetAll();
        
        var employeeDtos = _mapper.Map<List<EmployeeDto>>(employees);
        
        return employeeDtos;
    }

    public IEnumerable<EmployeeDto> SearchByRole(Role role)
    {
        var employees = _employeeRepository.SearchByRole(role);
        
        var employeeDtos = _mapper.Map<IEnumerable<EmployeeDto>>(employees);
        
        return employeeDtos;
    }

    public async Task<EmployeeDto> GetByEmailAsync(string email)
    {
        var employee = await _employeeRepository.GetByEmail(email);
        if (employee is null)
        {
            throw new EntityNotFoundException();
        }
        
        var employeeDto = _mapper.Map<EmployeeDto>(employee);
        
        return employeeDto;
    }

    public async Task AssignRoleAsync(int employeeId, Role role)
    {
        var employee = await _employeeRepository.GetById(employeeId);
        if (employee is null)
        {
            throw new EntityNotFoundException();
        }
        
        employee.Role = role;
        
        _employeeRepository.Update(employee);
        await _unitOfWork.SaveChangesAsync();
    }
    
    /// <exception cref="MethodAccessException"></exception>
    public IEnumerable<EmployeeDto> GetEmployeesFiltered(int pageNumber)
    {
        var user = SecurityContext.GetUser();
        if (user == null || !user.Roles.Contains(Role.Administrator))
        {
            throw new MethodAccessException();
        }

        var employeeId = user.UserId;
        var employees = _unitOfWork.Employees.Find(e => e.Id == employeeId, pageNumber, pageSize);

        var employeesDto = _mapper.Map<List<EmployeeDto>>(employees);
        return employeesDto;
    }
}