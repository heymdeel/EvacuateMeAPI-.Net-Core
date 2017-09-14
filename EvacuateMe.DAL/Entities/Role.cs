using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace EvacuateMe.DAL.Entities
{
    [Table("roles", Schema = "public")]
    public class Role : Entity
    {        
        [Column("name")]
        public string Name { get; set; }

        public virtual ICollection<User> Users { get; set; }

        public Role()
        {
            Users = new List<User>();
        }
    }
}
