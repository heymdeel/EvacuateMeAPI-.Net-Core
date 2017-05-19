using System;
using System.Collections.Generic;
using System.Text;

namespace EvacuateMe.BLL.Interfaces
{
    public interface IEncrypt
    {
        string GenerateHash(string arg1, string arg2);
        string GeneratePassword(string arg1, string arg2);
    }
}
