using BLL.DTOs;
using DAL.Entities;
using DAL.Enums;

namespace BLL.Services.Interfaces;

public interface IReportService
{
    Task CreatAsync(ReportDto reportDto);
    Task UpdateAsync(ReportDto reportDto);
    Task DeleteAsync(int employeeId);
    Task<ReportDto> GetByIdAsync(int employeeId);
    Task<IEnumerable<ReportDto>> GetAllAsync();
    Task UpdateStatusAsync(int reportId, ReportStatus status);
    Task AddContentAsync(int reportId, string content);
    Task MarkAsPrintedAsync(int reportId);
    Task AddIndicatorToReportAsync(int reportId, Indicator indicator);
    Task RemoveIndicatorFromReportAsync(int reportId, int indicatorId);
    Task<IEnumerable<IndicatorDto>> GetIndicatorsByReportIdAsync(int reportId);
    Task<IEnumerable<ReportDto>> GetReportsByEmployeeIdAsync(int employeeId);
    Task<IEnumerable<ReportDto>> GetByStatusAsync(ReportStatus status);
}