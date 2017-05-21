using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EvacuateMe.BLL.DTO
{
    public class SmsDTO
    {
        [Required, RegularExpression("^[7-8][0-9]{10}$")]
        public string Phone { get; set; }

        [Required, Range(1000, 9999, ErrorMessage = "Код должен принимать значения от 1000 до 9999")]
        public int Code { get; set; }
    }
}
