using System.ComponentModel.DataAnnotations;
public class UserResponseDto
{
    public Guid Id { get; set; }
    public string Email { get; set; } = null!;
    public string Name { get; set; } = null!;
}