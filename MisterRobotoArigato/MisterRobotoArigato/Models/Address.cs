using System.ComponentModel.DataAnnotations;

namespace MisterRobotoArigato.Models
{
    /// <summary>
    /// An address is connected to a user
    /// </summary>
    public class Address
    {
        public int ID { get; set; }

        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name ="Last Name")]
        public string LastName { get; set; }

        [Required]
        public string Street { get; set; }

        [Display(Name ="Street (cont.)")]
        public string Street2 { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public string State { get; set; }

        [Required]
        public string Country { get; set; }

        [Required]
        public string Zip { get; set; }

        [Required]
        public string UserID { get; set; }
    }
}