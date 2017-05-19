using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace EvacuateMe.DAL.Entities
{
    public enum WorkerStatus
    {
        Offline = 0,
        Working = 1,
        PerformingOrder = 2
    }
}
