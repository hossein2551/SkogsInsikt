using SkogsInsikt.Application.Auth;

namespace SkogsInsikt.Application.Interfaces;

public interface IJwtTokenService
{
    AuthResponse CreateToken(
        string userId,
        string email,
        string fullName,
        string role);
}
