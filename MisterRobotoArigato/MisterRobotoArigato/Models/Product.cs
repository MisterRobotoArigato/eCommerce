using System.ComponentModel.DataAnnotations;

namespace MisterRobotoArigato.Models
{
    /// <summary>
    /// Every product will get an ID, Name, SKU, Price, Description, and ImgUrl
    /// </summary>
    public class Product
    {
        public int ID { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string SKU { get; set; }

        public decimal Price { get; set; }

        public string Description { get; set; }

        [Display(Name = "Image")]
        public string ImgUrl { get; set; }
    }
}