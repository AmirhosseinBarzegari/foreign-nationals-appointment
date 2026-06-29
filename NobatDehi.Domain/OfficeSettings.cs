namespace NobatDehi.Domain.Entities;

public class OfficeSettings
{
    public int Id { get; set; }
    public int DailyCapacity { get; set; } //Number of clients that office can handle in a day
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }
    public int AppointmentDuration { get; set; } //The time assigned for each clients
    public string WorkingDays { get; set; } = string.Empty;

    // Relation with Office
    public int OfficeId { get; set; }
    public Office? Office { get; set; }
    

}
