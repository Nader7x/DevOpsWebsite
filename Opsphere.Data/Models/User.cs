using System.ComponentModel.DataAnnotations;

namespace Opsphere.Data.Models;


public class User
{
    public int Id { get; set; }
    public string Username { get; set; }
    [EmailAddress]
    public string Email { get; set; }
    
}