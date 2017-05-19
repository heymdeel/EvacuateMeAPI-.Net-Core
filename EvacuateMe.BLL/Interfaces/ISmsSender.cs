using System;
using System.Collections.Generic;
using System.Text;

namespace EvacuateMe.BLL.Interfaces
{
    public interface ISmsSender
    {
        void Invoke(string phone);
        void Dispose();
    }
}
