using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NobatDehi.Infrastructure.Data;
using NobatDehi.Domain.Entities;

namespace NobatDehi.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ProvinceController(AppDbContext context) : ControllerBase
{
    private readonly AppDbContext _context = context;
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var provinces = await _context.Provinces.ToListAsync();
        return Ok(provinces);
    }

    [HttpPost]
    public async Task<IActionResult> Create(Province province)
    {
        _context.Provinces.Add(province);
        await _context.SaveChangesAsync();

        return Ok(province);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, Province province)
    {
        var existingProvince = await _context.Provinces.FindAsync(id);
        if (existingProvince != null)
        {
            existingProvince.Name = province.Name;
            existingProvince.IsActive = province.IsActive;
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
        var deletedProvince = await _context.Provinces.FindAsync(id);
        if (deletedProvince != null)
        {
            _context.Provinces.Remove(deletedProvince);
            await _context.SaveChangesAsync();

            return Ok();
        }
        else
        {
            return NotFound();
        }
    }
}

