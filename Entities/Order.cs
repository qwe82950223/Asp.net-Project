using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Supreme.Entities
{
    [Table("Orders")]
    public class Order
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string UserName { get; set; }
        public int UserId { get; set; }
        public float TotalPrice { get; set; }
        public DateTime OderTime { get; set; }
       

        [ForeignKey("UserId")]
        public virtual Member Member { get; set; }

    }
}