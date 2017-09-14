using LinqToDB.Mapping;
using System;
using System.Collections.Generic;
using System.Text;

namespace EvacuateMe.DAL.Entities
{
    [Table("roles", Schema = "public")]
    public class Role : Entity
    {        
        [Column("name")]
        public string Name { get; set; }

        [Association(ThisKey = "Id", OtherKey = "RoleId")]
        public IEnumerable<User> Users { get; set; }

        public Role()
        {
            Users = new List<User>();
        }
    }
}
