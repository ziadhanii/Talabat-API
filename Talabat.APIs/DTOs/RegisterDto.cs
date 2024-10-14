namespace Talabat.APIs.DTOs;

public class RegisterDto
{
    [Required] public string DisplayName { get; set; }
    [Required] [EmailAddress] public string Email { get; set; }
    [Required] public string PhoneNumber { get; set; }

    [Required(ErrorMessage = "Password is required")]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[#!$%&'*+/=?^_`{|}~.-]).{8,}$",
        ErrorMessage =
            "Password must be at least 8 characters long, contain at least one uppercase letter, one lowercase letter, one number, and one special character.")]

    public string Password { get; set; }
}