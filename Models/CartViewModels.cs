using Supreme.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Supreme.Models
{
    public class CartViewModels
    {

        public int Id { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductSize { get; set; }
        public int maxQty { get; set; }
        public float Price { get; set; }
        public int Quantity { get; set; }
        public string ImageName { get; set; }
        public float Total { get { return Quantity * Price; } }


    }
}