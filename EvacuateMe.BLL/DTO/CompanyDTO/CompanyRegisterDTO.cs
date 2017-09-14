using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EvacuateMe.BLL.DTO
{
    public class CompanyRegisterDTO
    {
        [Display(Name = "Название")]
        [Required, StringLength(20, MinimumLength = 3, ErrorMessage = "Длина названия должна быть от 3 до 20 символов")]
        public string Name { get; set; }

        [Display(Name = "Описание")]
        [Required, StringLength(150, MinimumLength = 5, ErrorMessage = "Длина описания должна быть от 5 до 150 символов")]
        public string Description { get; set; }

        [Display(Name = "Адрес")]
        [Required, StringLength(20, MinimumLength = 3, ErrorMessage = "Длина адреса должна быть от 3 до 20 символов")]
        public string Address { get; set; }

        [Display(Name = "Номер телефона")]
        [Required, RegularExpression("^[7-8][0-9]{10}$")]
        public string ContactPhone { get; set; }

        [Display(Name = "Электроный адрес")]
        [Required, StringLength(25, MinimumLength = 5, ErrorMessage = "Длина почтового адреса должна быть от 5 до 25 символов")]
        public string EMail { get; set; }

        [Display(Name = "Минимальная сумма")]
        [Required, Range(50, 1000, ErrorMessage = "Минимальная сумма должна быть от 50 до 1000")]
        public double MinSum { get; set; }

        [Display(Name = "Тариф")]
        [Required, Range(50, 10000, ErrorMessage = "Тариф должен быть от 50 до 10000")]
        public double Tariff { get; set; }

        [Display(Name = "Логотип")]
        [Required, StringLength(200, MinimumLength = 5)]
        public string LogoUrl { get; set; }

        [Display(Name = "Логин")]
        [Required, StringLength(15, MinimumLength = 3, ErrorMessage = "Длина логина должна быть от 3 до 15 символов")]
        public string Login { get; set; }

        [Display(Name = "Пароль")]
        [Required]
        public string Password { get; set; }
    }
}
