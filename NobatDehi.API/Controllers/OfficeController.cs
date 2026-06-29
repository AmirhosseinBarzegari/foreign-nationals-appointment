using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NobatDehi.Infrastructure.Data;
using NobatDehi.Domain.Entities;
using NobatDehi.Application.DTOs;

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

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var office = await _context.Offices
        .Include(o => o.Province)
        .FirstOrDefaultAsync(o => o.Id == id);

        if (office == null)
            return NotFound();
            
        return Ok(office);
    }

    [HttpPost]
    public async Task<IActionResult> Create(Office office)
    {
        var exists = await _context.Offices.
                        AnyAsync(o => o.Name == office.Name
                                || o.Address == office.Address
                                && o.ProvinceId == office.ProvinceId);
        if (exists)
            return BadRequest("دفتر با مشخصات فوق وجود دارد.");

        _context.Offices.Add(office);
        await _context.SaveChangesAsync();

        return Ok();

    }

    [HttpPost("with-settings")]
    public async Task<IActionResult> CreateWithSettings(CreateOfficeDto dto)
    {
        var office = new Office
        {
            Name = dto.Name,
            Address = dto.Address,
            IsActive = true,
            ProvinceId = dto.ProvinceId
        };

        _context.Offices.Add(office);
        await _context.SaveChangesAsync();

        var settings = new OfficeSettings
        {
            OfficeId = office.Id,
            DailyCapacity = dto.DailyCapacity,
            AppointmentDuration = dto.AppointmentDuration,
            StartTime = dto.StartTime,
            EndTime = dto.EndTime,
            WorkingDays = dto.WorkingDays
        };

        var exists = await _context.Offices.AnyAsync(o =>
            o.ProvinceId == office.ProvinceId &&
            (o.Name == office.Name || o.Address == office.Address)
        );
           

        _context.OfficeSettings.Add(settings);
        await _context.SaveChangesAsync();

        return Ok(office);
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
