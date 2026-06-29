using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NobatDehi.Infrastructure.Data;
using NobatDehi.Domain.Entities;

namespace NobatDehi.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class HolidayController(AppDbContext context) : ControllerBase
{
    private readonly AppDbContext _context = context;

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var holiday = await _context.Holidays.Include(o => o.Province).Include(o => o.Office).ToListAsync();
        return Ok(holiday);
    }

    [HttpPost]
    public async Task<IActionResult> Create(Holiday holiday)
    {
        if (holiday.Date <= DateOnly.FromDateTime(DateTime.Today))
        {
            return BadRequest("تاریخ معتبر نیست.");
        }

        var exists = await _context.Holidays.
                        AnyAsync(o => o.Date == holiday.Date);
        if (exists)
            return BadRequest("این تاریخ قبلا ثبت شده است.") ;               

        _context.Holidays.Add(holiday);
        await _context.SaveChangesAsync();
        return Ok();
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, Holiday holiday)
    {
        var existingHoliday = await _context.Holidays.FindAsync(id);
        if (existingHoliday != null)
        {
            if (holiday.Date <= DateOnly.FromDateTime(DateTime.Today))
            {
                return BadRequest("تاریخ معتبر نیست.");
            }

            var exists = await _context.Holidays.
                        AnyAsync(o => o.Date == holiday.Date);
            if (exists)
                return BadRequest("این تاریخ قبلا ثبت شده است.") ;   

            existingHoliday.ProvinceId = holiday.ProvinceId;
            existingHoliday.Reason = holiday.Reason;
            existingHoliday.OfficeId = holiday.OfficeId;
            existingHoliday.IsOfficial = holiday.IsOfficial;
            existingHoliday.Date = holiday.Date;

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
        var deletedHoliday = await _context.Holidays.FindAsync(id);
        if (deletedHoliday != null)
        {
            _context.Holidays.Remove(deletedHoliday);
            await _context.SaveChangesAsync();
            return Ok();
        }
        else
        {
            return NotFound();
        }
    }
}
