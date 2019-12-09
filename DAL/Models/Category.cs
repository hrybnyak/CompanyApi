using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DAL.Models
{
    public class Category:EntityBase
    {
        [Key]
        public override int Id { get; set; }
        [MaxLength(150)]
        [Required]
        public string Name { get; set; }
        public ICollection<Product> Products { get; set; }
       
    }
}
