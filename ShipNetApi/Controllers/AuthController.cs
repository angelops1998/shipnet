using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        string? macNormalizada = null;
        if (!string.IsNullOrWhiteSpace(dto.MacAddress) && dto.MacAddress != "00:00:00:00:00:00")
        {
            macNormalizada = dto.MacAddress.Replace('-', ':').ToUpper();
            var estudianteExistenteConMac = await _context.Estudiantes
                .FirstOrDefaultAsync(e => e.MacAddress == macNormalizada);

            if (estudianteExistenteConMac != null)
            {
                return BadRequest($"El dispositivo con dirección MAC '{macNormalizada}' ya se encuentra registrado con otro estudiante ({estudianteExistenteConMac.Nombre} {estudianteExistenteConMac.Apellido}). Solo se permite un estudiante por dispositivo físico.");
            }
        }

        var user = new ApplicationUser { UserName = dto.Email, Email = dto.Email, NombreCompleto = dto.Nombre + " " + dto.Apellido };
        var resultado = await _userManager.CreateAsync(user, dto.Password);
        if (!resultado.Succeeded)
            return BadRequest(resultado.Errors.First().Description);

        await _userManager.AddToRoleAsync(user, "Estudiante");
        var estudiante = new Estudiante
        {
            Nombre = dto.Nombre,
            Apellido = dto.Apellido,
            CI = dto.CI,
            ApplicationUserId = user.Id,
            Carrera = dto.Carrera,
            Semestre = dto.Semestre > 0 ? dto.Semestre : 1,
            MacAddress = macNormalizada
        };
        _context.Estudiantes.Add(estudiante);

        // Si se vinculó la MAC de una vez, asegurar que esté en la tabla de Equipos
        if (!string.IsNullOrEmpty(macNormalizada))
        {
            var equipoExistente = await _context.Equipos.FirstOrDefaultAsync(e => e.MacAddress == macNormalizada);
            if (equipoExistente == null)
            {
                _context.Equipos.Add(new Equipo
                {
                    Codigo = $"MOVIL-{dto.CI}",
                    MacAddress = macNormalizada,
                    Ubicacion = $"Móvil - {dto.Nombre} {dto.Apellido}"
                });
            }
        }

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
