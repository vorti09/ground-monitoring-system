using AutoMapper;
using BLL.DTOs;
using BLL.Services.Impl;
using BLL.Tests.Fake;
using CCL.Security;
using CCL.Security.Identity;
using DAL.UnitOfWork;
using Moq;

namespace BLL.Tests;

public class EmployeeServiceTests
{
    [Fact]
    public void Ctor_InputNull_ThrowArgumentNullException()
    {
        // Arrange
        IUnitOfWork nullUnitOfWork = null;
        var mockedMapper = new Mock<IMapper>();
        // Act
        // Assert
        Assert.Throws<ArgumentNullException>(() => new EmployeeService(nullUnitOfWork, mockedMapper.Object));
    }

    [Fact]
    public void GetEmployees_EmployeeFromDAL_CorrectMappingToOrderDTO()
    {
        // Arrange
        User user = new User(1, new List<Role>{ Role.Administrator });
        SecurityContext.SetUser(user);

        var expectedEmployeeDto = new EmployeeDto()
        {
            Id = 1,
            Name = "Name",
            Email = "email@email.com",
            PhoneNumber = "PhoneNumber"
        };
        
        var employeerServiceFake = new EmployeeServiceFake(expectedEmployeeDto);
        var actualService = employeerServiceFake.Get();

        // Act
        var actualEmployeeDto = actualService.GetEmployeesFiltered(0).First();

        // Assert
        Assert.True(
            actualEmployeeDto.Id == expectedEmployeeDto.Id &&
            actualEmployeeDto.Name == expectedEmployeeDto.Name &&
            actualEmployeeDto.Email == expectedEmployeeDto.Email &&
            actualEmployeeDto.PhoneNumber == expectedEmployeeDto.PhoneNumber
        );
    }
}