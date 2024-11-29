using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Demo_Code_First.Models
{
    public class User
    {
        [Key]
        public int userId { get; set; }

        [DisplayName("User name: ")]
        public string userName { get; set; }

        [Required]
        [DisplayName("Email: ")]
        public string email { get; set; }

        [DisplayName("Password: ")]
        public string password { get; set; }
    }
}
