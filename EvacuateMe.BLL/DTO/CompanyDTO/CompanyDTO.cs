using System;
using System.Collections.Generic;
using System.Text;

namespace EvacuateMe.BLL.DTO.CompanyDTO
{
    public class CompanyDTO
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public string Address { get; set; }

        public string ContactPhone { get; set; }

        public string EMail { get; set; }

        public double MinSum { get; set; }

        public double Tariff { get; set; }

        public string LogoUrl { get; set; }
        
        public double Rate { get; set; }
    }
}
