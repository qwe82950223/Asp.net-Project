using Supreme.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Supreme.Models
{
    public class CategoryViewModels
    {
        public CategoryViewModels()
        {

        }
        public CategoryViewModels(Category row)
        {
            Id = row.Id;
            Name = row.Name;
        }
        public int Id { get; set; }
        public string Name { get; set; }
    }
}