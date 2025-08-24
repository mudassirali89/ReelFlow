using System.ComponentModel.DataAnnotations;
namespace ReelFLow.DTO
{
    public class RegistrationDTO
    {
        [Required(ErrorMessage = "UserName is required.")]
        [StringLength(50)]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 6)]
        public string Password { get; set; }

        [Required]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
    }
}
