using DAL.Data;
using DAL.Entities;
using DAL.Enums;
using DAL.Repositories.Impl.Base;
using DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories.Impl;

public class IndicatorRepository : BaseRepository<Indicator>, IIndicatorRepository
{
    private readonly ApplicationDbContext _context;
    public IndicatorRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Indicator>> GetByType(IndicatorType type)
    {
        return await _context.Indicators.Where(i => i.Type == type).ToListAsync();
    }

    public async Task<IEnumerable<Indicator>> GetByDateRange(DateTime startDate, DateTime endDate)
    {
        return await _context.Indicators.Where(i => i.CollectedDate >= startDate && i.CollectedDate <= endDate).ToListAsync();
    }

    public async Task<IEnumerable<Indicator>> GetAboveValue(double minValue)
    {
        return await _context.Indicators.Where(i => i.Value >= minValue).ToListAsync();
    }

    public double GetAverageValue(IndicatorType type)
    {
        return _context.Indicators.Where(i => i.Type == type).Average(i => i.Value);
    }

    public double GetMaxValue(IndicatorType type)
    {
        return _context.Indicators.Where(i => i.Type == type).Max(i => i.Value);
    }
}