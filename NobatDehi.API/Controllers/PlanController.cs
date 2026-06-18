using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NobatDehi.API.Data;
using NobatDehi.API.Models;

namespace NobatDehi.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class PlanController(AppDbContext context) : ControllerBase
{
    private readonly AppDbContext _context = context;

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var plan = await _context.Plans.Include(o => o.Dependencies).ToListAsync();
        return Ok(plan);
    }

    [HttpPost]
    public async Task<IActionResult> Create(Plan plan)
    {
        _context.Plans.Add(plan);
        await _context.SaveChangesAsync();
        return Ok();
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, Plan plan)
    {
        var existingPlan = await _context.Plans.FindAsync(id);
        if (existingPlan != null)
        {
            existingPlan.AllowDuplicate = plan.AllowDuplicate;
            existingPlan.CodeType = plan.CodeType;
            existingPlan.StartDate = plan.StartDate;
            existingPlan.EndDate = plan.EndDate;
            existingPlan.IsActive = plan.IsActive;
            existingPlan.MaxDuplicateCount = plan.MaxDuplicateCount;
            existingPlan.Name = plan.Name;

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
        var deletedPlan = await _context.Plans.FindAsync(id);
        if (deletedPlan != null)
        {
            _context.Plans.Remove(deletedPlan);
            await _context.SaveChangesAsync();
            return Ok();
        }
        else
        {
            return NotFound();
        }
    }
}
