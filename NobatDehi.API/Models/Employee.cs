
using System.ComponentModel;

namespace NobatDehi.API.Models;

public class Employee
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;


    //Relation with their office
    public int OfficeId { get; set; }
    public Office Office { get; set; } = null!;
}
