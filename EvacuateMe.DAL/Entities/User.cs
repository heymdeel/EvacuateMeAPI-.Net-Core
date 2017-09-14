using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace EvacuateMe.DAL.Entities
{
    [Table("users", Schema = "public")]
    public class User : Entity
    {        
        [Column("company")]
        [ForeignKey("Company")]
        public int CompanyId { get; set; }

        [Column("login")]
        public string Login { get; set; }

        [Column("password")]
        public string Password { get; set; }

        [Column("role")]
        [ForeignKey("Role")]
        public int? RoleId { get; set; }

        public virtual Role Role { get; set; }

        public virtual Company Company { get; set; }
    }
}
