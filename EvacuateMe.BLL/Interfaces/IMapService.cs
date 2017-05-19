using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EvacuateMe.BLL.Interfaces
{
    public interface IMapService
    {
        Task<double> GetDistanceAsync(double lat1, double lon1, double lat2, double lon2);
        Task<string> GetDurationAsync(double lat1, double lon1, double lat2, double lon2);
    }
}
