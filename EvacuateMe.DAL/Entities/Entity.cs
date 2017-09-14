using LinqToDB.Mapping;
using System;
using System.Collections.Generic;
using System.Text;

namespace EvacuateMe.DAL.Entities
{
    public abstract class Entity
    {
        [Identity, PrimaryKey, Column("id")]
        public virtual int Id { get; set; }
    }
}
