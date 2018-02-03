using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Supreme.Entities
{
    [Table("ShoesSizes")]
    public class ShoesSizeTable
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [DisplayName("7")]
        public int seven { get; set; }
        [DisplayName("7.5")]
        public int sevenhalf { get; set; }
        [DisplayName("8")]
        public int eight { get; set; }
        [DisplayName("8.5")]
        public int eighthalf { get; set; }
        [DisplayName("9")]
        public int nine { get; set; }
        [DisplayName("9.5")]
        public int ninehalf { get; set; }
        [DisplayName("10")]
        public int ten { get; set; }
        [DisplayName("10.5")]
        public int tenhalf { get; set; }
        [DisplayName("11")]
        public int eleven { get; set; }
    }
}