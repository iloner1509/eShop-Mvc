using System.ComponentModel.DataAnnotations;

namespace eShop_Mvc.Models.AccountViewModels
{
    public class ChangePasswordViewModel
    {
        [Required(ErrorMessage = "Mật khẩu không được để trống !")]
        [DataType(DataType.Password)]
        public string CurrentPassword { get; set; }

        [Required(ErrorMessage = "Mật khẩu mới không được để trống !")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "Xác nhận mật khẩu không được để trống !")]
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "Hãy xác nhận lại mật khẩu !")]
        public string ConfirmNewPassword { get; set; }
    }
}