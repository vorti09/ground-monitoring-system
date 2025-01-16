using AutoMapper;
using BLL.DTOs;
using BLL.Exceptions;
using BLL.Services.Interfaces;
using DAL.Entities;
using DAL.Enums;
using DAL.Repositories.Interfaces;
using DAL.UnitOfWork;

namespace BLL.Services.Impl;

public class IndicatorService : IIndicatorService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IIndicatorRepository _indicatorRepository;
    private readonly IMapper _mapper;

    public IndicatorService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _indicatorRepository = unitOfWork.Indicators;
        _mapper = mapper;
    }
    
    public async Task CreateAsync(IndicatorDto indicatorDto)
    {
        var indicator = await _indicatorRepository.GetById(indicatorDto.Id);
        if (indicator is not null)
        {
            throw new EntityAlreadyExistsException();
        }
        
        var indicatorEntity = _mapper.Map<Indicator>(indicatorDto);
        
        await _indicatorRepository.Create(indicatorEntity);
        await _unitOfWork.SaveChangesAsync();
    }
    
    public async Task UpdateAsync(IndicatorDto indicatorDto)
    {
        var indicator = await _indicatorRepository.GetById(indicatorDto.Id);
        if (indicator is null)
        {
            throw new EntityNotFoundException();
        }
        
        var indicatorEntity = _mapper.Map<Indicator>(indicatorDto);
        
        _indicatorRepository.Update(indicatorEntity);
        await _unitOfWork.SaveChangesAsync();
    }
    
    public async Task DeleteAsync(int indicatorId)
    {
        var indicator = await _indicatorRepository.GetById(indicatorId);
        if (indicator is null)
        {
            throw new EntityNotFoundException();
        }
        
        await _indicatorRepository.Delete(indicator.Id);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<IndicatorDto> GetByIdAsync(int indicatorId)
    {
        var indicator = await _indicatorRepository.GetById(indicatorId);
        if (indicator is null)
        {
            throw new EntityNotFoundException();
        }
        
        var indicatorDto = _mapper.Map<IndicatorDto>(indicator);
        
        return indicatorDto;
    }
    
    public async Task<List<IndicatorDto>> GetAllAsync()
    {
        var indicators = await _indicatorRepository.GetAll();
        
        var indicatorsDtos = _mapper.Map<List<IndicatorDto>>(indicators);
        
        return indicatorsDtos;
    }

    public async Task<IEnumerable<IndicatorDto>> GetByTypeAsync(IndicatorType type)
    {
        var indicators = await _indicatorRepository.GetByType(type);
        
        var indicatorsDtos = _mapper.Map<IEnumerable<IndicatorDto>>(indicators);
        
        return indicatorsDtos;
    }

    public async Task<IEnumerable<IndicatorDto>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
    {
        var indicators = await _indicatorRepository.GetByDateRange(startDate, endDate);
        
        var indicatorsDtos = _mapper.Map<IEnumerable<IndicatorDto>>(indicators);
        
        return indicatorsDtos;
    }

    public async Task<IEnumerable<IndicatorDto>> GetAboveValueAsync(double value)
    {
        var indicators = await _indicatorRepository.GetAboveValue(value);
        
        var indicatorsDtos = _mapper.Map<IEnumerable<IndicatorDto>>(indicators);
        
        return indicatorsDtos;
    }

    public double GetAverageValueAsync(IndicatorType type)
    {
        var result = _indicatorRepository.GetAverageValue(type);
        
        return result;
    }

    public double GetMaxValueAsync(IndicatorType type)
    {
        var result = _indicatorRepository.GetMaxValue(type);
        
        return result;
    }
}