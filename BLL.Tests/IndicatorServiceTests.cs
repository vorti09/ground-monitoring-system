using AutoMapper;
using BLL.DTOs;
using BLL.Exceptions;
using BLL.Services.Impl;
using DAL.Entities;
using DAL.Enums;
using DAL.Repositories.Interfaces;
using DAL.UnitOfWork;
using Moq;

namespace BLL.Tests;

public class IndicatorServiceTests
{
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IIndicatorRepository> _mockIndicatorRepository;
    private readonly Mock<IMapper> _mockMapper;
    private readonly IndicatorService _service;

    public IndicatorServiceTests()
    {
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockIndicatorRepository = new Mock<IIndicatorRepository>();
        _mockMapper = new Mock<IMapper>();

        _mockUnitOfWork.Setup(u => u.Indicators).Returns(_mockIndicatorRepository.Object);

        _service = new IndicatorService(_mockUnitOfWork.Object, _mockMapper.Object);
    }

    [Fact]
    public async Task CreateAsync_ShouldCreateIndicator_WhenIndicatorDoesNotExist()
    {
        // Arrange
        var indicatorDto = new IndicatorDto { Id = 1 };
        var indicatorEntity = new Indicator();

        _mockIndicatorRepository.Setup(r => r.GetById(indicatorDto.Id))
            .ReturnsAsync((Indicator)null);

        _mockMapper.Setup(m => m.Map<Indicator>(indicatorDto))
            .Returns(indicatorEntity);

        // Act
        await _service.CreateAsync(indicatorDto);

        // Assert
        _mockIndicatorRepository.Verify(r => r.Create(indicatorEntity), Times.Once);
        _mockUnitOfWork.Verify(u => u.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_ShouldThrowException_WhenIndicatorAlreadyExists()
    {
        // Arrange
        var indicatorDto = new IndicatorDto { Id = 1 };
        var existingIndicator = new Indicator { Id = 1 };

        _mockIndicatorRepository.Setup(r => r.GetById(indicatorDto.Id))
            .ReturnsAsync(existingIndicator);

        // Act & Assert
        await Assert.ThrowsAsync<EntityAlreadyExistsException>(() => _service.CreateAsync(indicatorDto));
        _mockIndicatorRepository.Verify(r => r.Create(It.IsAny<Indicator>()), Times.Never);
        _mockUnitOfWork.Verify(u => u.SaveChangesAsync(), Times.Never);
    }

    [Fact]
    public async Task DeleteAsync_ShouldDeleteIndicator_WhenIndicatorExists()
    {
        // Arrange
        var indicatorId = 1;
        var indicator = new Indicator { Id = indicatorId };

        _mockIndicatorRepository.Setup(r => r.GetById(indicatorId))
            .ReturnsAsync(indicator);

        // Act
        await _service.DeleteAsync(indicatorId);

        // Assert
        _mockIndicatorRepository.Verify(r => r.Delete(indicatorId), Times.Once);
        _mockUnitOfWork.Verify(u => u.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_ShouldThrowException_WhenIndicatorDoesNotExist()
    {
        // Arrange
        var indicatorId = 1;

        _mockIndicatorRepository.Setup(r => r.GetById(indicatorId))
            .ReturnsAsync((Indicator)null);

        // Act & Assert
        await Assert.ThrowsAsync<EntityNotFoundException>(() => _service.DeleteAsync(indicatorId));
        _mockIndicatorRepository.Verify(r => r.Delete(It.IsAny<int>()), Times.Never);
        _mockUnitOfWork.Verify(u => u.SaveChangesAsync(), Times.Never);
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateIndicator_WhenIndicatorExists()
    {
        // Arrange
        var indicatorDto = new IndicatorDto { Id = 1, Name = "Updated Name" };
        var existingIndicator = new Indicator { Id = 1, Name = "Old Name" };
        var updatedIndicator = new Indicator { Id = 1, Name = "Updated Name" };

        _mockIndicatorRepository.Setup(r => r.GetById(indicatorDto.Id))
            .ReturnsAsync(existingIndicator);

        _mockMapper.Setup(m => m.Map<Indicator>(indicatorDto))
            .Returns(updatedIndicator);

        // Act
        await _service.UpdateAsync(indicatorDto);

        // Assert
        _mockIndicatorRepository.Verify(r => r.Update(updatedIndicator), Times.Once);
        _mockUnitOfWork.Verify(u => u.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_ShouldThrowException_WhenIndicatorDoesNotExist()
    {
        // Arrange
        var indicatorDto = new IndicatorDto { Id = 1 };

        _mockIndicatorRepository.Setup(r => r.GetById(indicatorDto.Id))
            .ReturnsAsync((Indicator)null);

        // Act & Assert
        await Assert.ThrowsAsync<EntityNotFoundException>(() => _service.UpdateAsync(indicatorDto));
        _mockIndicatorRepository.Verify(r => r.Update(It.IsAny<Indicator>()), Times.Never);
        _mockUnitOfWork.Verify(u => u.SaveChangesAsync(), Times.Never);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnIndicator_WhenIndicatorExists()
    {
        // Arrange
        var indicatorId = 1;
        var indicator = new Indicator { Id = indicatorId };
        var indicatorDto = new IndicatorDto { Id = indicatorId };

        _mockIndicatorRepository.Setup(r => r.GetById(indicatorId))
            .ReturnsAsync(indicator);

        _mockMapper.Setup(m => m.Map<IndicatorDto>(indicator))
            .Returns(indicatorDto);

        // Act
        var result = await _service.GetByIdAsync(indicatorId);

        // Assert
        Assert.Equal(indicatorDto, result);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldThrowException_WhenIndicatorDoesNotExist()
    {
        // Arrange
        var indicatorId = 1;

        _mockIndicatorRepository.Setup(r => r.GetById(indicatorId))
            .ReturnsAsync((Indicator)null);

        // Act & Assert
        await Assert.ThrowsAsync<EntityNotFoundException>(() => _service.GetByIdAsync(indicatorId));
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllIndicators()
    {
        // Arrange
        var indicators = new List<Indicator>
        {
            new Indicator { Id = 1 },
            new Indicator { Id = 2 }
        };

        var indicatorsDtos = new List<IndicatorDto>
        {
            new IndicatorDto { Id = 1 },
            new IndicatorDto { Id = 2 }
        };

        _mockIndicatorRepository.Setup(r => r.GetAll())
            .ReturnsAsync(indicators);

        _mockMapper.Setup(m => m.Map<List<IndicatorDto>>(indicators))
            .Returns(indicatorsDtos);

        // Act
        var result = await _service.GetAllAsync();

        // Assert
        Assert.Equal(indicatorsDtos, result);
    }

    [Fact]
    public async Task GetByTypeAsync_ShouldReturnIndicatorsOfSpecifiedType()
    {
        // Arrange
        var type = IndicatorType.CO2;
        var indicators = new List<Indicator> { new Indicator { Id = 1, Type = type } };
        var indicatorsDtos = new List<IndicatorDto> { new IndicatorDto { Id = 1 } };

        _mockIndicatorRepository.Setup(r => r.GetByType(type))
            .ReturnsAsync(indicators);

        _mockMapper.Setup(m => m.Map<IEnumerable<IndicatorDto>>(indicators))
            .Returns(indicatorsDtos);

        // Act
        var result = await _service.GetByTypeAsync(type);

        // Assert
        Assert.Equal(indicatorsDtos, result);
    }
}