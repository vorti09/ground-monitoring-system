using BLL.DTOs;
using DAL.Enums;

namespace BLL.Services.Interfaces;

public interface IIndicatorService
{
    Task CreateAsync(IndicatorDto indicatorDto);
    Task UpdateAsync(IndicatorDto indicatorDto);
    Task DeleteAsync(int indicatorId);
    Task<IndicatorDto> GetByIdAsync(int indicatorId);
    Task<List<IndicatorDto>> GetAllAsync();
    Task<IEnumerable<IndicatorDto>> GetByTypeAsync(IndicatorType type);
    Task<IEnumerable<IndicatorDto>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);
    Task<IEnumerable<IndicatorDto>> GetAboveValueAsync(double value);
    double GetAverageValueAsync(IndicatorType type);
    double GetMaxValueAsync(IndicatorType type);
}