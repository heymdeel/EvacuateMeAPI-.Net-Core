using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace EvacuateMe.DAL.Entities
{
    public abstract class Entity
    {
        [Key, Column("id")]
        public virtual int Id { get; set; }

    }
}
