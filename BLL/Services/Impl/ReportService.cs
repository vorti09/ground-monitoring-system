using System.Reflection.Metadata;
using AutoMapper;
using BLL.DTOs;
using BLL.Exceptions;
using BLL.Services.Interfaces;
using DAL.Entities;
using DAL.Enums;
using DAL.Repositories.Interfaces;
using DAL.UnitOfWork;
using iText.Kernel.Pdf;
using iText.Layout.Element;

namespace BLL.Services.Impl;

public class ReportService : IReportService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IReportRepository _reportRepository;
    private readonly IMapper _mapper;

    public ReportService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _reportRepository = unitOfWork.Reports;
        _mapper = mapper;
    }
    
    public async Task CreatAsync(ReportDto reportDto)
    {
        var report = await _reportRepository.GetById(reportDto.Id);
        if (report is not null)
        {
            throw new EntityAlreadyExistsException();
        }
        
        var reportEntity = _mapper.Map<Report>(reportDto);
        
        await _reportRepository.Create(reportEntity);
        await _unitOfWork.SaveChangesAsync();
    }
    
    public async Task UpdateAsync(ReportDto reportDto)
    {
        var report = await _reportRepository.GetById(reportDto.Id);
        if (report is null)
        {
            throw new EntityNotFoundException();
        }
        
        var reportEntity = _mapper.Map<Report>(reportDto);
        
        _reportRepository.Update(reportEntity);
        await _unitOfWork.SaveChangesAsync();
    }
    
    public async Task DeleteAsync(int employeeId)
    {
        var report = await _reportRepository.GetById(employeeId);
        if (report is null)
        {
            throw new EntityNotFoundException();
        }
        
        await _reportRepository.Delete(report.Id);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<ReportDto> GetByIdAsync(int employeeId)
    {
        var report = await _reportRepository.GetById(employeeId);
        if (report is null)
        {
            throw new EntityNotFoundException();
        }
        
        var reportDto = _mapper.Map<ReportDto>(report);
        
        return reportDto;
    }

    public async Task<IEnumerable<ReportDto>> GetAllAsync()
    {
        var medicalReports = await _reportRepository.GetAll();
        
        var medicalReportsDto = _mapper.Map<IEnumerable<ReportDto>>(medicalReports);
        
        return medicalReportsDto;
    }

    public async Task UpdateStatusAsync(int reportId, ReportStatus status)
    {
        var report = await _reportRepository.GetById(reportId);
        if (report is null)
        {
            throw new EntityNotFoundException();
        }
        
        report.Status = status;
        
        _reportRepository.Update(report);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task AddContentAsync(int reportId, string content)
    {
        var report = await _reportRepository.GetById(reportId);
        if (report is null)
        {
            throw new EntityNotFoundException();
        }
        
        report.Content = content;
        
        _reportRepository.Update(report);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task MarkAsPrintedAsync(int reportId)
    {
        var report = await _reportRepository.GetById(reportId);
        if (report is null)
        {
            throw new EntityNotFoundException();
        }
        
        report.IsPrinted = true;
        
        _reportRepository.Update(report);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task AddIndicatorToReportAsync(int reportId, Indicator indicator)
    {
        var report = await _reportRepository.GetById(reportId);
        if (report is null)
        {
            throw new EntityNotFoundException();
        }
        
        report.Indicators.Add(indicator);
        
        _reportRepository.Update(report);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task RemoveIndicatorFromReportAsync(int reportId, int indicatorId)
    {
        var report = await _reportRepository.GetById(reportId);
        if (report is null)
        {
            throw new EntityNotFoundException();
        }
        
        var indicator = report.Indicators.FirstOrDefault(i => i.Id == indicatorId);
        if (indicator is null)
        {
            throw new EntityNotFoundException();
        }
        
        report.Indicators.Remove(indicator);
        
        _reportRepository.Update(report);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<IEnumerable<IndicatorDto>> GetIndicatorsByReportIdAsync(int reportId)
    {
        var report = await _reportRepository.GetById(reportId);
        if (report is null)
        {
            throw new EntityNotFoundException();
        }
        
        var indicatorsDto = _mapper.Map<IEnumerable<IndicatorDto>>(report.Indicators);
        
        return indicatorsDto;
    }

    public async Task<IEnumerable<ReportDto>> GetReportsByEmployeeIdAsync(int employeeId)
    {
        var reports = await _reportRepository.GetReportsByEmployeeId(employeeId);
        
        var reportsDto = _mapper.Map<IEnumerable<ReportDto>>(reports);
        
        return reportsDto;
    }

    public async Task<IEnumerable<ReportDto>> GetByStatusAsync(ReportStatus status)
    {
        var reports = await _reportRepository.GetByStatus(status);
        
        var reportsDto = _mapper.Map<IEnumerable<ReportDto>>(reports);
        
        return reportsDto;
    }
}