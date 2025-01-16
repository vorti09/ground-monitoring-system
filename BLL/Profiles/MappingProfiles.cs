using AutoMapper;
using BLL.DTOs;
using DAL.Entities;

namespace BLL.Profiles;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<Employee, EmployeeDto>().ReverseMap();
        CreateMap<Indicator, IndicatorDto>().ReverseMap();
        CreateMap<Report, ReportDto>().ReverseMap();
    }
}