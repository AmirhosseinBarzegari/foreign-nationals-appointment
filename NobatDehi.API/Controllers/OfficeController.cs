using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NobatDehi.API.Data;
using NobatDehi.API.Models;

namespace NobatDehi.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class OfficeController(AppDbContext context) : ControllerBase
{
    private readonly AppDbContext _context = context;

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var offices = await _context.Offices
        .Include(o => o.Province)
        .ToListAsync();
        return Ok(offices);
    }

    [HttpPost]
    public async Task<IActionResult> Create(Office office)
    {
        _context.Offices.Add(office);
        await _context.SaveChangesAsync();

        return Ok();

    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, Office office)
    {
        var existingOffice = await _context.Offices.FindAsync(id);
        if (existingOffice != null)
        {
            existingOffice.Address = office.Address;
            existingOffice.IsActive = office.IsActive;
            existingOffice.Name = office.Name;
            existingOffice.ProvinceId = office.ProvinceId;

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
        var deletedOffice = await _context.Offices.FindAsync(id);
        if (deletedOffice != null)
        {
            _context.Offices.Remove(deletedOffice);
            await _context.SaveChangesAsync();

            return Ok();
        }
        else
        {
            return NotFound();
        }
    }
}
