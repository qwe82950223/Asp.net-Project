using Supreme.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Supreme.Models
{
    public class AccountViewModels
    {
        public AccountViewModels()
        {
        }

        public AccountViewModels(Member row)
        {
            UserName = row.UserName;
            Email = row.EmailAddress;
            PhoneNumber = row.PhoneNumber;
            LastName = row.LastName;
            FirstName = row.FirstName;
            Address = row.Address;
            Password = row.Password;
            Photo = row.PhotoName;
        }


        public string UserName { get; set; }
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Address { get; set; }
        public string Password { get; set; }
        public string Photo { get; set; }
    }
}