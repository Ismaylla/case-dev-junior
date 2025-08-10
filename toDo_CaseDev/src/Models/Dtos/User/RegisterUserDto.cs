using System.ComponentModel.DataAnnotations;

public class RegisterUserDto
{
    [Required, EmailAddress]
    public string Email { get; set; } = null!;

    [Required, MinLength(2), MaxLength(50)]
    public string Name { get; set; } = null!;

    [Required, MinLength(8)]
    public string Password { get; set; } = null!;
}
