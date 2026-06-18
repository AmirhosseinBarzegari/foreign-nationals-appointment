using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NobatDehi.API.Data;
using NobatDehi.API.Models;

namespace NobatDehi.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class OfficeHolidayExceptionController(AppDbContext context) : ControllerBase
{
    private readonly AppDbContext _context = context;

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var exceptions = await _context.OfficeHolidayExceptions.
                        Include(o => o.Office).
                        Include(o => o.Holiday).ToListAsync();

        return Ok(exceptions);
    }

    [HttpPost]
    public async Task<IActionResult> Create(OfficeHolidayException officeHolidayException)
    {
        var office = await _context.Offices.FindAsync(officeHolidayException.OfficeId);
        var holiday = await _context.Holidays.FindAsync(officeHolidayException.HolidayId);

        if (office != null && holiday != null)
        {
            _context.OfficeHolidayExceptions.Add(officeHolidayException);
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
        var deleteItem = await _context.OfficeHolidayExceptions.FindAsync(id);
        if (deleteItem != null)
        {
            _context.OfficeHolidayExceptions.Remove(deleteItem);
            await _context.SaveChangesAsync();
            return Ok();
        }
        else
        {
            return NotFound();
        }
    }

}
