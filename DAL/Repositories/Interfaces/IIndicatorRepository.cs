using DAL.Entities;
using DAL.Enums;

namespace DAL.Repositories.Interfaces;

public interface IIndicatorRepository : IRepository<Indicator>
{
    Task<IEnumerable<Indicator>> GetByType(IndicatorType type);
    Task<IEnumerable<Indicator>> GetByDateRange(DateTime startDate, DateTime endDate);
    Task<IEnumerable<Indicator>> GetAboveValue(double minValue);
    double GetAverageValue(IndicatorType type);
    double GetMaxValue(IndicatorType type);
}