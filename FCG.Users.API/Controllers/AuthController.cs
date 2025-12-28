using FCG.Users.API.Extensions;
using FCG.Users.API.Filters;
using FCG.Users.Domain.DTOs.Requests;
using FCG.Users.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace FCG.Users.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController(IAuthService authService) : ControllerBase
    {
        /// <summary>
        /// Realiza o login de um usuário no sistema.
        /// </summary>
        /// <response code="200">Usuário autenticado com sucesso.</response>
        /// <response code="401">Credenciais inválidas</response>
        [HttpPost("login", Name = "Login")]
        [ServiceFilter(typeof(ValidationFilter<LoginRequest>))]
        public async Task<IActionResult> Login([FromBody] LoginRequest request, CancellationToken ct)
        {
            var result = await authService.LoginAsync(request, ct);

            return result.ToActionResult();
        }
    }
}
