using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SkogsInsikt.Application.Auth;
using SkogsInsikt.Application.Interfaces;
using SkogsInsikt.Infrastructure.Identity;

namespace SkogsInsikt.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IJwtTokenService _jwtTokenService;

    public AuthController(
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager,
        IJwtTokenService jwtTokenService)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _jwtTokenService = jwtTokenService;
    }

    [HttpPost("register")]
    public async Task<ActionResult<AuthResponse>> Register(
        RegisterRequest request)
    {
        var existingUser =
            await _userManager.FindByEmailAsync(request.Email);

        if (existingUser is not null)
        {
            return Conflict(new
            {
                message = "En användare med den e-postadressen finns redan."
            });
        }

        var user = new ApplicationUser
        {
            UserName = request.Email,
            Email = request.Email,
            FullName = request.FullName
        };

        var result =
            await _userManager.CreateAsync(user, request.Password);

        if (!result.Succeeded)
        {
            return BadRequest(new
            {
                errors = result.Errors.Select(error => error.Description)
            });
        }

        const string role = "User";

        if (!await _roleManager.RoleExistsAsync(role))
        {
            var roleResult =
                await _roleManager.CreateAsync(new IdentityRole(role));

            if (!roleResult.Succeeded)
            {
                await _userManager.DeleteAsync(user);

                return StatusCode(500, new
                {
                    message = "Kunde inte skapa användarrollen."
                });
            }
        }

        var addToRoleResult =
            await _userManager.AddToRoleAsync(user, role);

        if (!addToRoleResult.Succeeded)
        {
            await _userManager.DeleteAsync(user);

            return StatusCode(500, new
            {
                message = "Kunde inte tilldela användarrollen."
            });
        }

        return Ok(
            _jwtTokenService.CreateToken(
                user.Id,
                user.Email!,
                user.FullName,
                role));
    }

    [HttpPost("login")]
    public async Task<ActionResult<AuthResponse>> Login(
        LoginRequest request)
    {
        var user =
            await _userManager.FindByEmailAsync(request.Email);

        if (user is null ||
            !await _userManager.CheckPasswordAsync(
                user,
                request.Password))
        {
            return Unauthorized(new
            {
                message = "Felaktig e-postadress eller lösenord."
            });
        }

        var roles = await _userManager.GetRolesAsync(user);
        var role = roles.FirstOrDefault() ?? "User";

        return Ok(
            _jwtTokenService.CreateToken(
                user.Id,
                user.Email!,
                user.FullName,
                role));
    }
}
