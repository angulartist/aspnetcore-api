using System.ComponentModel.DataAnnotations;

namespace dotnetFun.API.Dtos
{
    public class UserForRegisterDto
    {
        [Required]
        public string Username { get; set; }
        [Required]
        [StringLength(16, MinimumLength = 8, ErrorMessage = "Password trop court (8)")]
        public string Password { get; set; }
    }
}