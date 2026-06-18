using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NobatDehi.API.Data;
using NobatDehi.API.DTOs;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace NobatDehi.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IConfiguration _configuration;

    public AuthController(AppDbContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto dto)
    {
        // Find employee by username
        var employee = await _context.Employees
            .FirstOrDefaultAsync(e => e.UserName == dto.Username && e.IsActive);


        // If user not found
        if (employee == null)
            return Unauthorized("Username or password is incorrect");

        // If password was not correct
        if (!BCrypt.Net.BCrypt.Verify(dto.Password, employee.Password))
            return Unauthorized("Username or password is incorrect");

        // make token
        var token = GenerateToken(employee.Id, employee.UserName);

        return Ok(new { token });
    }


    private string GenerateToken(int id, string username)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, id.ToString()),
            new Claim(ClaimTypes.Name, username)
        };

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.Now.AddDays(int.Parse(_configuration["Jwt:ExpireDays"]!)),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}