namespace NobatDehi.Domain.Entities;

public class PlanDependency
{
    public int Id { get; set; }
    public int PlanId { get; set; }
    public Plan? Plan { get; set; } = null!;
    public int RequiredPlanId { get; set; }
    public Plan? RequiredPlan { get; set; } = null!;
}