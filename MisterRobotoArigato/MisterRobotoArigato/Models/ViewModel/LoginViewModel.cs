using System.ComponentModel.DataAnnotations;

namespace MisterRobotoArigato.Models.ViewModel
{
    /// <summary>
    /// For the purposes of Login not with an 3rd party OAUTh
    /// </summary>
    public class LoginViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}