using EvacuateMe.BLL.Interfaces;
using EvacuateMe.DAL.Entities;
using EvacuateMe.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EvacuateMe.BLL.Services
{
    public class SmsService : ISmsSender
    {
        static Random rnd = new Random();

        private readonly IUnitOfWork db;

        public SmsService(IUnitOfWork unitOfWork)
        {
            db = unitOfWork;
        }

        public async Task InvokeAsync(string phone)
        {
            int code = rnd.Next(1000, 9999);
            var sms = await db.SMSCodes.FirstOrDefaultAsync(s => s.Phone == phone);

            if (sms == null)
            {
                await db.SMSCodes.CreateAsync(new SMSCode() { Phone = phone, Code = code, TimeStamp = DateTime.Now });
            }
            else
            {
                sms.Code = code;
                sms.TimeStamp = DateTime.Now;
                await db.SMSCodes.UpdateAsync(sms);
            }

            Console.WriteLine($"Sending sms code == {code} on number {phone}");
        }
    }
}
