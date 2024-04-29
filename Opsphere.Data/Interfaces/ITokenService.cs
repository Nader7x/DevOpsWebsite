using Opsphere.Data.Models;

namespace Opsphere.Data.Interfaces;

public interface ITokenService
{
    String CreateToken(User user);
}