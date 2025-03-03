using System.ComponentModel.DataAnnotations;

namespace HMS_WebAPI.DTOs
{
    public class UserDTO
    {
        [Required] public string UserName { get; set; }
        [Required] public string Password { get; set; }

        [Required] [Compare(nameof(Password))] public string PasswordConf { get; set; }
        public List<int> RoleIds { get; set; }

    }
}
