using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Supreme.Entities
{
    [Table("Members")]
    public class Member
    { 
            [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
            public int Id { get; set; }
            [Required]
            [MaxLength(255)]
            public string UserName { get; set; }
            [MaxLength(255)]
            public string LastName { get; set; }
            [MaxLength(255)]
            public string FirstName { get; set; }
            [Required]
            [MaxLength(255)]
            public string EmailAddress { get; set; }
            [MaxLength(2056)]
            public string Address { get; set; }
            [MaxLength(255)]
            public string PhoneNumber { get; set; }
            public string Password { get; set; }
            [MaxLength(255)]
            public string PhotoName { get; set; }
            public int RoleId { get; set; }

        [ForeignKey("RoleId")]
        public virtual MemberRole MemberRole { get; set; }




    }
}