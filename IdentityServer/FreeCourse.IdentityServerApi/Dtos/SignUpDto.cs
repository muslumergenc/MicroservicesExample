using System.ComponentModel.DataAnnotations;

namespace FreeCourse.IdentityServerApi.Dtos
{
    public class SignUpDto
    {  [Required]
        public string? UserName { get; set; }
        [Required]
        public string? Email { get; set; }
        [Required]
        public string? Password { get; set; }
        public string? City { get; set; }
    }
}
