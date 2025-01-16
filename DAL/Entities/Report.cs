using DAL.Enums;

namespace DAL.Entities;

public class Report
{
    public int Id { get; set; }
    public int EmployeeId { get; set; }
    public Employee Employee { get; set; }
    public DateTime CreateDate { get; set; }
    public string Content { get; set; }
    public bool IsPrinted { get; set; }
    public ReportStatus Status { get; set; }
    public ICollection<Indicator> Indicators { get; set; } = new List<Indicator>();
}