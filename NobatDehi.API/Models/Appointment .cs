namespace NobatDehi.API.Models;

public class Appointment
{
    public int Id { get; set; }
    public string ForeignerCode { get; set; } = null!;
    public DateOnly AppointmentDate { get; set; }
    public TimeOnly AppointmentTime { get; set; }
    public string Status { get; set; } = null!;
    public int PlanId { get; set; }
    public Plan? Plan { get; set; }
    public int OfficeId { get; set; }
    public Office? Office { get; set; }
}