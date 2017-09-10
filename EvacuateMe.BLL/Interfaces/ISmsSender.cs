using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EvacuateMe.BLL.Interfaces
{
    public interface ISmsSender
    {
        Task InvokeAsync(string phone);
    }
}
