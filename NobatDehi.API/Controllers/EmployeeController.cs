using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NobatDehi.API.Data;
using NobatDehi.API.Models;

namespace NobatDehi.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class EmployeeController(AppDbContext context) : ControllerBase
{
    private readonly AppDbContext _context = context;

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var employees = await _context.Employees
        .Include(e => e.Office)
        .ThenInclude(o => o.Province)
        .ToListAsync();
        return Ok(employees);
    }

    [HttpPost]
    public async Task<IActionResult> Create(Employee employee)
    {
        employee.Password = BCrypt.Net.BCrypt.HashPassword(employee.Password);
        _context.Employees.Add(employee);
        await _context.SaveChangesAsync();
        return Ok();
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, Employee employee)
    {
        var existingEmployee = await _context.Employees.FindAsync(id);
        if (existingEmployee != null)
        {
            existingEmployee.FirstName = employee.FirstName;
            existingEmployee.LastName = employee.LastName;
            existingEmployee.IsActive = employee.IsActive;
            existingEmployee.Password = employee.Password;
            existingEmployee.UserName = employee.UserName;
            existingEmployee.OfficeId = employee.OfficeId;
            

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
        var deletedEmployee = await _context.Employees.FindAsync(id);
        if (deletedEmployee != null)
        {
            _context.Employees.Remove(deletedEmployee);
            await _context.SaveChangesAsync();

            return Ok();
        }
        else
        {
            return NotFound();
        }
    }

}
