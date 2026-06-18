namespace NobatDehi.API.Models;

public class OfficeHolidayException 
{
    public int Id { get; set; }
    public DateOnly Date { get; set; }
    // Relation with office
    public int OfficeId { get; set; }
    public Office Office { get; set; } = null!;
    // Relation with holiday
    public int HolidayId { get; set; }
    public Holiday Holiday { get; set; } = null!;
}
