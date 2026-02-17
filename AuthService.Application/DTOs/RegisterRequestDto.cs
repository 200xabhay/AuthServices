using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Application.DTOs
{
    public class RegisterRequestDto
    {
        [Required(ErrorMessage = "Please Enter Your Name")]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "Please Enter Your Gender")]

        public string Gender { get; set; } = null!;
        [Required(ErrorMessage = "Please Enter Your Email")]

        [DataType(DataType.EmailAddress)]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Please Enter Your Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;

        public int RoleId { get; set; } = 1;
    }
}
