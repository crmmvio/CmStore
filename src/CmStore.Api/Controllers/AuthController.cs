using CmStore.Core.Data;
using CmStore.Core.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CmStore.Api.Controllers
{
    [ApiController]
    [Route("api/conta")]
    public class AuthController : ControllerBase
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly AppDbContext _context;
        private readonly JwtSettings _jwtSettings;

        public AuthController(SignInManager<IdentityUser> signInManager,
                              UserManager<IdentityUser> userManager,
                              AppDbContext context,
                              IOptions<JwtSettings> jwtSettings)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _context = context;
            _jwtSettings = jwtSettings.Value;
        }

        [HttpPost("registrar")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status422UnprocessableEntity)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> Registrar(RegisterUserViewModel registerUser)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            var user = new IdentityUser
            {
                Email = registerUser.Email,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(user, registerUser.Password);

            if (result.Succeeded)
            {
                var vendedor = new Vendedor
                {
                    Id = user.Id,
                    Nome = registerUser.Name,
                    Email = user.Email,
                    Ativo = true
                };

                _context.Vendedores.Add(vendedor);
                await _context.SaveChangesAsync();

                await _signInManager.SignInAsync(user, false);
                return Ok(await GerarJwt(registerUser.Email));
            }

            return ValidationProblem("Falha ao registrar o usuário");
        }

        [HttpPost("login")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status422UnprocessableEntity)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> Login(LoginUserViewModel loginUser)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            var result = await _signInManager.PasswordSignInAsync(loginUser.Email, loginUser.Password, false, true);

            if (result.Succeeded)
            {
                return Ok(await GerarJwt(loginUser.Email));
            }

            return Problem("Usuário ou senha incorretos");
        }

        private async Task<string> GerarJwt(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            var roles = await _userManager.GetRolesAsync(user);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Id)
            };

            //Adicionando as roles do usuário como Claims
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSettings.Segredo);

            var token = tokenHandler.CreateToken(new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Issuer = _jwtSettings.Emissor,
                Audience = _jwtSettings.Audiencia,
                Expires = DateTime.UtcNow.AddHours(_jwtSettings.ExpiracaoHoras),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            });

            var encodedToken = tokenHandler.WriteToken(token);

            return encodedToken;
        }
    }
}
