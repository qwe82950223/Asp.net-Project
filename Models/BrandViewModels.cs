using Supreme.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Supreme.Models
{
    public class BrandViewModels
    {
        public BrandViewModels()
        {

        }
        public BrandViewModels(Brand row)
        {
            Id = row.Id;
            Name = row.Name;
        }
        public int Id { get; set; }
        public string Name { get; set; }
    }
}