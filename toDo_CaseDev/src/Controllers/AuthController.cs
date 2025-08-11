using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly AuthService _authService;
    private readonly UserService _userService;
    private readonly ILogger<AuthController> _logger;

    public AuthController(AuthService authService, UserService userService, ILogger<AuthController> logger)
    {
        _authService = authService;
        _userService = userService;
        _logger = logger;
    }

    [HttpPost("register")]
    public IActionResult Register([FromBody] RegisterUserDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var user = _userService.CreateUser(dto.Email, dto.Name, dto.Password);

            var response = new UserResponseDto
            {
                Id = user.Id,
                Email = user.Email,
                Name = user.Name
            };

            return Created("", response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro interno ao registrar usuário com email {Email}.", dto.Email);
            return StatusCode(500, new { error = "Ocorreu um erro ao processar sua solicitação." });
        }
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginUserDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var user = _authService.ValidateUser(dto.Email, dto.Password);

            if (user == null)
                return Unauthorized(new { message = "Credenciais inválidas." });

            var token = _authService.GenerateJwtToken(user);
            return Ok(new { access_token = token });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro interno ao tentar logar usuário com email {Email}.", dto.Email);
            return StatusCode(500, new { error = "Ocorreu um erro ao processar sua solicitação." });
        }
    }
}
