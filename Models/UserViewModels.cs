using Supreme.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Supreme.Models
{
    public class UserViewModels
    {
        public UserViewModels()
        {
        }

        public UserViewModels(Member row)
        {
            Id = row.Id;
            UserName = row.UserName;
            Email = row.EmailAddress;
            PhoneNumber = row.PhoneNumber;
            LastName = row.LastName;
            FirstName = row.FirstName;
            Address = row.Address;
            Password = row.Password;
            Photo = row.PhotoName;
            RoleId = row.RoleId;
        }

        public int Id { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Address { get; set; }
        [Required]
        public string Password { get; set; }
        public string ComfirmPassword { get; set; }
        public string Photo { get; set; }
        public int RoleId { get; set; }

        public IEnumerable<SelectListItem> RoleList { get; set; }

        public string RoleName { get; set; }
    }
}