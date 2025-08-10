using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly AuthService _authService;
    private readonly UserService _userService;

    public AuthController(AuthService authService, UserService userService)
    {
        _authService = authService;
        _userService = userService;
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
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginUserDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var user = _authService.ValidateUser(dto.Email, dto.Password);

        if (user == null)
            return Unauthorized(new { message = "Credenciais inválidas." });

        var token = _authService.GenerateJwtToken(user);
        return Ok(new { access_token = token });
    }
}
