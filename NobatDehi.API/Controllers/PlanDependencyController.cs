using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NobatDehi.API.Data;
using NobatDehi.API.Models;

namespace NobatDehi.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class PlanDependencyController (AppDbContext context) : ControllerBase
{
    private readonly AppDbContext _context = context;

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var item = await _context.PlanDependencies.ToListAsync();
        return Ok(item);
    }

    [HttpPost]
    public async Task<IActionResult> Create(PlanDependency planDependency)
    {
        var plan = await _context.Plans.FindAsync(planDependency.PlanId);
        var requiredPlan = await _context.Plans.FindAsync(planDependency.RequiredPlanId);

        if (plan != null && requiredPlan != null)
        {
            _context.PlanDependencies.Add(planDependency);
            await _context.SaveChangesAsync();
            return Ok();
        }
        else
        {
            return NotFound();
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var dependency = await _context.PlanDependencies.FindAsync(id);
        if (dependency == null) return NotFound();
        _context.PlanDependencies.Remove(dependency);
        await _context.SaveChangesAsync();
        return Ok();
    }

}
