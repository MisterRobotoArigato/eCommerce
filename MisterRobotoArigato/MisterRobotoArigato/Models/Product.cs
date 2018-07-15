using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MisterRobotoArigato.Models
{
    public class Product
    {
        public int ID { get; set; }
        
        [Required]
        public string Name { get; set; }

        [Required]
        public string SKU { get; set; }

        public decimal Price { get; set; }

        public string Description { get; set; }

        [Display(Name="Image")]
        public string ImgUrl { get; set; }
    }
}