using AutoMapper;
using BLL.DTOs;
using BLL.Services.Impl;
using BLL.Services.Interfaces;
using DAL.Entities;
using DAL.Repositories.Interfaces;
using DAL.UnitOfWork;
using Microsoft.Extensions.Logging;
using Moq;

namespace BLL.Tests.Fake;

public class EmployeeServiceFake
{
    private EmployeeDto employeeDto;

    public EmployeeServiceFake(EmployeeDto employeeDto)
    {
        this.employeeDto = employeeDto;
    }
    
    public IEmployeeService Get()
    {
        var mockUnitOfWork = new Mock<IUnitOfWork>();
        var mockMapper = new Mock<IMapper>();

        var expectedEmployee = new Employee()
        {
            Id = employeeDto.Id,
            Name = employeeDto.Name,
            PhoneNumber = employeeDto.PhoneNumber,
            Email = employeeDto.Email
        };

        var mockEmployeeRepository = new Mock<IEmployeeRepository>();
        mockEmployeeRepository
            .Setup(repo => repo.Find(It.IsAny<Func<Employee, bool>>(), It.IsAny<int>(), It.IsAny<int>()))
            .Returns(new List<Employee> { expectedEmployee });
        
        mockUnitOfWork.Setup(u => u.Employees).Returns(mockEmployeeRepository.Object);
        
        mockMapper
            .Setup(m => m.Map<List<EmployeeDto>>(It.IsAny<List<Employee>>()))
            .Returns(new List<EmployeeDto> { employeeDto });
        
        IEmployeeService employeeService = new EmployeeService(mockUnitOfWork.Object, mockMapper.Object);
        return employeeService;
    }
}