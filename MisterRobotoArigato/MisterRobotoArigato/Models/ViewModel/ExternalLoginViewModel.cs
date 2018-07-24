using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MisterRobotoArigato.Models.ViewModel
{
    /// <summary>
    /// For the purposes of using an OAUTH login
    /// </summary>
    public class ExternalLoginViewModel
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [EmailAddress]
        [Required]
        public string Email { get; set; }
    }
}
