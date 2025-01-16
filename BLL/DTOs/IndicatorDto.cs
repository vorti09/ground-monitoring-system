using DAL.Enums;

namespace BLL.DTOs;

public class IndicatorDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public double Value { get; set; }
    public IndicatorType Type { get; set; }
    public DateTime CollectedDate { get; set; }
}