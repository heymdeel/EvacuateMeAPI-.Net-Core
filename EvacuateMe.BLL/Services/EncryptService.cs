using EvacuateMe.BLL.Interfaces;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace EvacuateMe.BLL.Services
{
    public class EncryptService : IEncrypt
    {
        public string GenerateHash(string arg1, string arg2)
        {
            MD5 md5 = MD5.Create();

            byte[] inputBytes = Encoding.ASCII.GetBytes(arg1 + "some_salt" + arg2);
            byte[] hash = md5.ComputeHash(inputBytes);

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("x2"));
            }

            return sb.ToString();
        }

        public string GeneratePassword(string arg1, string arg2)
        {
            throw new NotImplementedException();
        }
    }
}
