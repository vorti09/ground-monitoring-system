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

public class ReportServiceTests
{
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IReportRepository> _mockReportRepository;
    private readonly Mock<IMapper> _mockMapper;
    private readonly ReportService _service;

    public ReportServiceTests()
    {
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockReportRepository = new Mock<IReportRepository>();
        _mockMapper = new Mock<IMapper>();

        _mockUnitOfWork.Setup(u => u.Reports).Returns(_mockReportRepository.Object);

        _service = new ReportService(_mockUnitOfWork.Object, _mockMapper.Object);
    }

    [Fact]
    public async Task CreateAsync_ShouldCreateReport_WhenReportDoesNotExist()
    {
        // Arrange
        var reportDto = new ReportDto { Id = 1 };
        _mockReportRepository.Setup(r => r.GetById(reportDto.Id)).ReturnsAsync((Report)null);
        var reportEntity = new Report { Id = 1 };

        _mockMapper.Setup(m => m.Map<Report>(reportDto)).Returns(reportEntity);

        // Act
        await _service.CreatAsync(reportDto);

        // Assert
        _mockReportRepository.Verify(r => r.Create(reportEntity), Times.Once);
        _mockUnitOfWork.Verify(u => u.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_ShouldThrowException_WhenReportAlreadyExists()
    {
        // Arrange
        var reportDto = new ReportDto { Id = 1 };
        var existingReport = new Report { Id = 1 };

        _mockReportRepository.Setup(r => r.GetById(reportDto.Id)).ReturnsAsync(existingReport);

        // Act & Assert
        await Assert.ThrowsAsync<EntityAlreadyExistsException>(() => _service.CreatAsync(reportDto));
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateReport_WhenReportExists()
    {
        // Arrange
        var reportDto = new ReportDto { Id = 1 };
        var existingReport = new Report { Id = 1 };
        var updatedReport = new Report { Id = 1 };

        _mockReportRepository.Setup(r => r.GetById(reportDto.Id)).ReturnsAsync(existingReport);
        _mockMapper.Setup(m => m.Map<Report>(reportDto)).Returns(updatedReport);

        // Act
        await _service.UpdateAsync(reportDto);

        // Assert
        _mockReportRepository.Verify(r => r.Update(updatedReport), Times.Once);
        _mockUnitOfWork.Verify(u => u.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_ShouldThrowException_WhenReportDoesNotExist()
    {
        // Arrange
        var reportDto = new ReportDto { Id = 1 };

        _mockReportRepository.Setup(r => r.GetById(reportDto.Id)).ReturnsAsync((Report)null);

        // Act & Assert
        await Assert.ThrowsAsync<EntityNotFoundException>(() => _service.UpdateAsync(reportDto));
    }

    [Fact]
    public async Task DeleteAsync_ShouldDeleteReport_WhenReportExists()
    {
        // Arrange
        var reportId = 1;
        var existingReport = new Report { Id = reportId };

        _mockReportRepository.Setup(r => r.GetById(reportId)).ReturnsAsync(existingReport);

        // Act
        await _service.DeleteAsync(reportId);

        // Assert
        _mockReportRepository.Verify(r => r.Delete(reportId), Times.Once);
        _mockUnitOfWork.Verify(u => u.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_ShouldThrowException_WhenReportDoesNotExist()
    {
        // Arrange
        var reportId = 1;

        _mockReportRepository.Setup(r => r.GetById(reportId)).ReturnsAsync((Report)null);

        // Act & Assert
        await Assert.ThrowsAsync<EntityNotFoundException>(() => _service.DeleteAsync(reportId));
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnReport_WhenReportExists()
    {
        // Arrange
        var reportId = 1;
        var report = new Report { Id = reportId };
        var reportDto = new ReportDto { Id = reportId };

        _mockReportRepository.Setup(r => r.GetById(reportId)).ReturnsAsync(report);
        _mockMapper.Setup(m => m.Map<ReportDto>(report)).Returns(reportDto);

        // Act
        var result = await _service.GetByIdAsync(reportId);

        // Assert
        Assert.Equal(reportDto, result);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldThrowException_WhenReportDoesNotExist()
    {
        // Arrange
        var reportId = 1;

        _mockReportRepository.Setup(r => r.GetById(reportId)).ReturnsAsync((Report)null);

        // Act & Assert
        await Assert.ThrowsAsync<EntityNotFoundException>(() => _service.GetByIdAsync(reportId));
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllReports()
    {
        // Arrange
        var reports = new List<Report> { new Report { Id = 1 }, new Report { Id = 2 } };
        var reportsDto = new List<ReportDto> { new ReportDto { Id = 1 }, new ReportDto { Id = 2 } };

        _mockReportRepository.Setup(r => r.GetAll()).ReturnsAsync(reports);
        _mockMapper.Setup(m => m.Map<IEnumerable<ReportDto>>(reports)).Returns(reportsDto);

        // Act
        var result = await _service.GetAllAsync();

        // Assert
        Assert.Equal(reportsDto, result);
    }

    [Fact]
    public async Task UpdateStatusAsync_ShouldUpdateReportStatus_WhenReportExists()
    {
        // Arrange
        var reportId = 1;
        var status = ReportStatus.Approved;
        var report = new Report { Id = reportId, Status = ReportStatus.Rejected };

        _mockReportRepository.Setup(r => r.GetById(reportId)).ReturnsAsync(report);

        // Act
        await _service.UpdateStatusAsync(reportId, status);

        // Assert
        Assert.Equal(status, report.Status);
        _mockReportRepository.Verify(r => r.Update(report), Times.Once);
        _mockUnitOfWork.Verify(u => u.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task AddContentAsync_ShouldAddContent_WhenReportExists()
    {
        // Arrange
        var reportId = 1;
        var content = "New Content";
        var report = new Report { Id = reportId };

        _mockReportRepository.Setup(r => r.GetById(reportId)).ReturnsAsync(report);

        // Act
        await _service.AddContentAsync(reportId, content);

        // Assert
        Assert.Equal(content, report.Content);
        _mockReportRepository.Verify(r => r.Update(report), Times.Once);
        _mockUnitOfWork.Verify(u => u.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task AddContentAsync_ShouldThrowException_WhenReportDoesNotExist()
    {
        // Arrange
        var reportId = 1;
        var content = "New Content";

        _mockReportRepository.Setup(r => r.GetById(reportId)).ReturnsAsync((Report)null);

        // Act & Assert
        await Assert.ThrowsAsync<EntityNotFoundException>(() => _service.AddContentAsync(reportId, content));
    }

    [Fact]
    public async Task MarkAsPrintedAsync_ShouldMarkAsPrinted_WhenReportExists()
    {
        // Arrange
        var reportId = 1;
        var report = new Report { Id = reportId, IsPrinted = false };

        _mockReportRepository.Setup(r => r.GetById(reportId)).ReturnsAsync(report);

        // Act
        await _service.MarkAsPrintedAsync(reportId);

        // Assert
        Assert.True(report.IsPrinted);
        _mockReportRepository.Verify(r => r.Update(report), Times.Once);
        _mockUnitOfWork.Verify(u => u.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task MarkAsPrintedAsync_ShouldThrowException_WhenReportDoesNotExist()
    {
        // Arrange
        var reportId = 1;

        _mockReportRepository.Setup(r => r.GetById(reportId)).ReturnsAsync((Report)null);

        // Act & Assert
        await Assert.ThrowsAsync<EntityNotFoundException>(() => _service.MarkAsPrintedAsync(reportId));
    }
    
}