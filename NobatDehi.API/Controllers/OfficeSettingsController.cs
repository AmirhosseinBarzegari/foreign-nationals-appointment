using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NobatDehi.API.Data;
using NobatDehi.API.Models;

namespace NobatDehi.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class OfficeSettingsController(AppDbContext context) : ControllerBase
{
    private readonly AppDbContext _context = context;

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var officeSettings = await _context.OfficeSettings.ToListAsync();

        return Ok(officeSettings);
    }

    [HttpPost]
    public async Task<IActionResult> Create(OfficeSettings officeSettings)
    {
        _context.OfficeSettings.Add(officeSettings);
        await _context.SaveChangesAsync();
        return Ok();
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, OfficeSettings officeSetting)
    {
        var existingOfficeSetting = await _context.OfficeSettings.FindAsync(id);
        if (existingOfficeSetting != null)
        {
            existingOfficeSetting.AppointmentDuration = officeSetting.AppointmentDuration;
            existingOfficeSetting.DailyCapacity = officeSetting.DailyCapacity;
            existingOfficeSetting.StartTime = officeSetting.StartTime;
            existingOfficeSetting.EndTime = officeSetting.EndTime;
            existingOfficeSetting.OfficeId = officeSetting.OfficeId;
            existingOfficeSetting.WorkingDays = officeSetting.WorkingDays;

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
        var deletedOfficeSetting = await _context.OfficeSettings.FindAsync(id);
        if (deletedOfficeSetting != null)
        {
            _context.OfficeSettings.Remove(deletedOfficeSetting);
            await _context.SaveChangesAsync();

            return Ok();
        }
        else
        {
            return NotFound();
        }
    }
}
