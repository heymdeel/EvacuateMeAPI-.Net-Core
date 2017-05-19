using EvacuateMe.BLL.Interfaces;
using EvacuateMe.DAL.Entities;
using EvacuateMe.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace EvacuateMe.BLL.Services
{
    public class GRequest : ISmsSender
    {
        static Random rnd = new Random();

        private readonly IUnitOfWork db;

        public GRequest(IUnitOfWork unitOfWork)
        {
            db = unitOfWork;
        }

        public void Invoke(string phone)
        {
            int code = rnd.Next(1000, 9999);
            var sms = db.SMSCodes.FirstOrDefault(s => s.Phone == phone);

            if (sms == null)
            {
                db.SMSCodes.Create(new SMSCode() { Phone = phone, Code = code, TimeStamp = DateTime.Now });
            }
            else
            {
                sms.Code = code;
                sms.TimeStamp = DateTime.Now;
                db.SMSCodes.Update(sms);
            }

            Console.WriteLine($"Sending sms code == {code} on number {phone}");
        }

        public void Dispose()
        {
            db.Dispose();
        }
    }
}
