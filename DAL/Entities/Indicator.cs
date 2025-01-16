using DAL.Enums;

namespace DAL.Entities;

public class Indicator
{
    public int Id { get; set; }
    public string Name { get; set; }
    public double Value { get; set; }
    public IndicatorType Type { get; set; }
    public DateTime CollectedDate { get; set; }
}