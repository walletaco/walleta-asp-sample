using System.ComponentModel.DataAnnotations;
using Walleta.Cpg.SampleCode.Helpers;

namespace Walleta.Cpg.SampleCode.Models
{
    public class SimplePayModel
    {
        [Required(ErrorMessage = "*")]
        [Range(minimum:10000,maximum:int.MaxValue,ErrorMessage = "حداقل مبلغ 10000 تومان می باشد")]
        public int Amount { get; set; }

        [Required(ErrorMessage = "*")]
        [NationalCode(ErrorMessage = "کد ملی وارد شده معتبر نمی باشد")]
        public string NationalCode { get; set; }

        [Required(ErrorMessage = "*")]
        [Mobile(ErrorMessage = "فرمت شماره موبایل 09120000000")]
        public string Mobile { get; set; }
    }
}