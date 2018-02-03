using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Supreme.Entities
{
    [Table("Products")]
    public class Product
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [MaxLength(255)]
        [Required]
        public string ProductName { get; set; }
        [MaxLength(2048)]
        public string Description { get; set; }
        public float Price { get; set; }
        public string ProductType { get; set; }
        public DateTime addTime { get; set; }
        public int ClothSizeId { get; set; }
        public int ShoesSizeId { get; set; }
        public int BrandId { get; set;}
        public int CategoryId { get; set; }
        [MaxLength(255)]
        public string ImageName { get; set; }

        [ForeignKey("CategoryId")]
        public virtual Category Category { get; set; }

        [ForeignKey("BrandId")]
        public virtual Brand Brand { get; set; }

        [ForeignKey("ClothSizeId")]
        public virtual ClothSizeTable ClothSizeTable { get; set; }

        [ForeignKey("ShoesSizeId")]
        public virtual ShoesSizeTable ShoesSizeTable { get; set; }

    }
}