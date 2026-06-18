namespace NobatDehi.API.Models;

public class Office
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Address { get; set; }
    public bool IsActive { get; set; } = true;

    // Relation with province
    public int ProvinceId { get; set; }
    public Province Province { get; set; } = null!;
}
