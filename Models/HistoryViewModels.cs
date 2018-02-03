using Supreme.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Supreme.Models
{
    public class HistoryViewModels
    {
        public HistoryViewModels()
        {
        }

        public HistoryViewModels(OrderDetail row)
        {
            OrderNumber = row.OrderId;
            ProductName = row.ProductName;
            Quantity = row.Quantity;
            Price = row.Price;
        }
        public int OrderNumber { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public float Price { get; set; }

    }
}