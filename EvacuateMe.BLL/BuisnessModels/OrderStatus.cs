using System;
using System.Collections.Generic;
using System.Text;

namespace EvacuateMe.BLL.BuisnessModels
{
    internal enum OrderStatus
    {
        Awaiting = 0,
        OnTheWay = 1,
        Performing = 2,
        Completed = 3,
        CanceledByWorker = 4,
        CanceledByClient = 5
    }
}
