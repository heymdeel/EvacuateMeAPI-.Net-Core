using LinqToDB.Mapping;
using System;
using System.Collections.Generic;
using System.Text;

namespace EvacuateMe.DAL.Entities
{
    [Table("users", Schema = "public")]
    public class User : Entity
    {        
        [Column("company")]
        public int CompanyId { get; set; }

        [Column("login")]
        public string Login { get; set; }

        [Column("password")]
        public string Password { get; set; }

        [Column("role")]
        public int? RoleId { get; set; }

        public Role Role { get; set; }

        public Company Company { get; set; }
    }
}
