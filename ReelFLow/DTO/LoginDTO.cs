using System.ComponentModel.DataAnnotations;
namespace ReelFLow.DTO
{
    public class LoginDTO
    {
        [Required(ErrorMessage = "Username or Email is required.")]
        [StringLength(100, ErrorMessage = "Username or Email cannot exceed 100 characters.")]
        public string UsernameOrEmail { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [DataType(DataType.Password)]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters.")]
        public string Password { get; set; }
    }
}
