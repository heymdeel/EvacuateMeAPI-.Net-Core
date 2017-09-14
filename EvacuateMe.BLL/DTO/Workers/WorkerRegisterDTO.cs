using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EvacuateMe.BLL.DTO
{
    public class WorkerRegisterDTO
    {
        [Display(Name = "Имя")]
        [Required, StringLength(15, MinimumLength = 3, ErrorMessage = "Длина имени должна быть от 3 до 15 символов")]
        public string Name { get; set; }

        [Display(Name = "Номер телефона")]
        [Required, RegularExpression("^[7-8][0-9]{10}$")]
        public string Phone { get; set; }

        [Display(Name = "Фамиилия")]
        [Required, StringLength(15, MinimumLength = 3, ErrorMessage = "Длина фамилии должна быть от 3 до 15 символов")]
        public string Surname { get; set; }

        [Display(Name = "Отчество")]
        [Required, StringLength(15, MinimumLength = 3, ErrorMessage = "Длина отчества должна быть от 3 до 15 символов")]
        public string Patronymic { get; set; }

        [Display(Name = "Номер автомобиля")]
        [Required, StringLength(15, MinimumLength = 3, ErrorMessage = "Длина номера машины должна быть от 3 до 15 символов")]
        public string CarNumber { get; set; }
    }
}
