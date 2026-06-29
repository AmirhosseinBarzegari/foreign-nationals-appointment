namespace NobatDehi.Application.DTOs;

public class CreateOfficeDto
{
    public string Name { get; set; } = null!;
    public string Address { get; set; } = null!;
    public int ProvinceId { get; set; }
    public int DailyCapacity { get; set; }
    public int AppointmentDuration { get; set; }
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }
    public string WorkingDays { get; set; } = null!;
}