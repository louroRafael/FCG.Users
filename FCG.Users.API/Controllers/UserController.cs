using FCG.Users.API.Extensions;
using FCG.Users.API.Filters;
using FCG.Users.Domain.DTOs.Requests;
using FCG.Users.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FCG.UsersF.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController(IUserService service) : ControllerBase
    {
        /// <summary>
        /// Cria um novo usuário padrão no sistema.
        /// </summary>
        /// <response code="200">Usuário criado com sucesso</response>
        [HttpPost("create-user")]
        [AllowAnonymous]
        [ServiceFilter(typeof(ValidationFilter<CreateUserRequest>))]
        public async Task<IActionResult> Create([FromBody] CreateUserRequest request, CancellationToken ct)
        {
            var created = await service.CreateAsync(request, ct);
            return created.ToActionResult();
        }

        /// <summary>
        /// Cria um novo usuário administrador no sistema.
        /// </summary>
        /// <response code="200">Usuário criado com sucesso</response>
        [HttpPost("create-admin")]
        [Authorize(Roles = "Admin")]
        [ServiceFilter(typeof(ValidationFilter<CreateAdminRequest>))]
        public async Task<IActionResult> CreateAdmin([FromBody] CreateAdminRequest request, CancellationToken ct)
        {
            var created = await service.CreateAdminAsync(request, ct);
            return created.ToActionResult();
        }

        /// <summary>
        /// Lista todos os usuários cadastrados
        /// </summary>
        /// <response code="200">Retorna a lista de todos os usuários encontrados.</response>
        [HttpGet("users")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAll(CancellationToken ct)
        {
            var users = await service.GetAllUsersAsync(ct);
            return users.ToActionResult();
        }
    }
}
