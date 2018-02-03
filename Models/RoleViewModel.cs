using Supreme.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Supreme.Models
{
    public class RoleViewModel
    {
        public RoleViewModel()
        {

        }
        public RoleViewModel(MemberRole row)
        {
            Id = row.Id;
            RoleName = row.RoleName;
        }
        public int Id { get; set; }
        public string RoleName { get; set; }
    }
}