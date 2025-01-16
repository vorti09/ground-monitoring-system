using DAL.Enums;

namespace BLL.DTOs;

public class ReportDto
{
    public int Id { get; set; }
    public int EmployeeId { get; set; }
    public DateTime CreateDate { get; set; }
    public string Content { get; set; }
    public bool IsPrinted { get; set; }
    public ReportStatus Status { get; set; }
}