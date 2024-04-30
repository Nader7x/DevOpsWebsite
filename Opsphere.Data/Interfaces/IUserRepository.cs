using Opsphere.Data.Models;

namespace Opsphere.Data.Interfaces;

public interface IUserRepository : IBaseRepository<User>
{
    Task<bool> Checkusername(string username);
    Task<User?> Getbyusername(string? username);
}