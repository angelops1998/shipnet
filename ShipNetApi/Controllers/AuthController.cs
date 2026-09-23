using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ShipNetApi.Data;
using ShipNetApi.Dtos;
using ShipNetApi.Models;

namespace ShipNetApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ApplicationDbContext _context;
    private readonly IConfiguration _config;

    public AuthController(UserManager<ApplicationUser> userManager, ApplicationDbContext context, IConfiguration config)
    {
        _userManager = userManager;
        _context = context;
        _config = config;
    }

    // POST: api/Auth/login
    [HttpPost("login")]
    public async Task<ActionResult<AuthResponseDto>> Login(LoginDto dto)
    {
        var user = await _userManager.FindByEmailAsync(dto.Email);
        if (user == null || !await _userManager.CheckPasswordAsync(user, dto.Password))
            return Unauthorized("Correo o contraseña incorrectos.");

        return Ok(await CrearRespuesta(user));
    }

    // POST: api/Auth/register  (solo estudiantes se registran solos)
    [HttpPost("register")]
    public async Task<ActionResult<AuthResponseDto>> Register(RegisterDto dto)
    {
        var user = new ApplicationUser { UserName = dto.Email, Email = dto.Email, NombreCompleto = dto.Nombre + " " + dto.Apellido };
        var resultado = await _userManager.CreateAsync(user, dto.Password);
        if (!resultado.Succeeded)
            return BadRequest(resultado.Errors.First().Description);

        await _userManager.AddToRoleAsync(user, "Estudiante");
        _context.Estudiantes.Add(new Estudiante { Nombre = dto.Nombre, Apellido = dto.Apellido, CI = dto.CI, ApplicationUserId = user.Id });
        await _context.SaveChangesAsync();

        return Ok(await CrearRespuesta(user));
    }

    private async Task<AuthResponseDto> CrearRespuesta(ApplicationUser user)
    {
        var roles = await _userManager.GetRolesAsync(user);
        var jwt = _config.GetSection("Jwt");
        var expira = DateTime.UtcNow.AddHours(jwt.GetValue<int>("HorasExpiracion"));

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id),
            new(ClaimTypes.Name, user.Email!)
        };
        claims.AddRange(roles.Select(r => new Claim(ClaimTypes.Role, r)));

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt["Key"]!));
        var token = new JwtSecurityToken(
            issuer: jwt["Issuer"],
            audience: jwt["Audience"],
            claims: claims,
            expires: expira,
            signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256));

        return new AuthResponseDto
        {
            Token = new JwtSecurityTokenHandler().WriteToken(token),
            Expira = expira,
            Email = user.Email!,
            NombreCompleto = user.NombreCompleto,
            Roles = roles.ToList()
        };
    }
}
