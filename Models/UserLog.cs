using System.ComponentModel.DataAnnotations;
namespace Login_Registration.Models
{
    public class UserLog
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        public string LoginEmail { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string LoginPassword { get; set; }
    }
}