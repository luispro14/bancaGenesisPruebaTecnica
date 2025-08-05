using bancaGenesisAPIs.Data;
using bancaGenesisAPIs.Models;
using BCrypt.Net;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Text;

namespace bancaGenesisAPIs.Controllers
{
    [ApiController]
    [Route("api/auth/[controller]")]
    public class UsuariosController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public UsuariosController(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public class LoginRequest
        {
            public string Email { get; set; }
            public string Password { get; set; }

        }

        private string GenerateJwtToken(Usuario usuaio)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Email, usuaio.Email)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var usuario = await _context.Usuarios.FirstOrDefaultAsync( u => u.Email == request.Email );


            if (usuario == null || !BCrypt.Net.BCrypt.Verify(request.Password, usuario.Password))
            {
                return Unauthorized(new {mensaje =  "Credenciales inválidas"});
            }


            //Si todo sale bien genera un Token

            var token = GenerateJwtToken(usuario);

            return Ok(new { token });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] LoginRequest request)
        {
            var usuario = await _context.Usuarios.AnyAsync(u => u.Email == request.Email && u.Password == request.Password);

            if (usuario)
            {
                return Unauthorized(new { mensaje = "Usuario ya existe" });
            }

            var registroUsuario = new Usuario
            {
                Email = request.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(request.Password)
            };

            _context.Usuarios.Add(registroUsuario);

            await _context.SaveChangesAsync();

            return Ok(usuario);
        }



    }
}
