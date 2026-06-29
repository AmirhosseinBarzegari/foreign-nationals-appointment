namespace NobatDehi.Domain.Entities;

public class OfficeHolidayException
{
    public int Id { get; set; }
    // Relation with office
    public int OfficeId { get; set; }
    public Office? Office { get; set; } = null!;
    // Relation with holiday
    public int HolidayId { get; set; }
    public Holiday? Holiday { get; set; } = null!;
}