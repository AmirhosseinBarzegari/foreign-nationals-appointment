namespace NobatDehi.Domain.Entities;

public class Plan
{
    public int Id { get; set; }
    public bool IsActive { get; set; }
    public string Name { get; set; } = null!;
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public string CodeType { get; set; } = null!; // Type of ID code required from foreigner (e.g. Passport, Yekta, Faragir)
    public bool AllowDuplicate { get; set; } // Can a foreigner with duplicate code take another appointment?
    public int MaxDuplicateCount { get; set; } // Maximum number of times a duplicate code can take appointment
    public List<PlanDependency> Dependencies { get; set; } = new();
}