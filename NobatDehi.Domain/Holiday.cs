namespace NobatDehi.Domain.Entities;

public class Holiday
{
    public int Id { get; set; }
    public DateOnly Date { get; set; }
    public string Reason { get; set; } = null!;
    public bool IsOfficial { get; set; }
    public int? ProvinceId { get; set; } // Special for a province, can be null that mean it is global
    public int? OfficeId { get; set; } // Special for an office, can be null that mean it is global
    // Relation with province or office, if holiday is not global
    public Office? Office { get; set; }
    public Province? Province { get; set; }
}
