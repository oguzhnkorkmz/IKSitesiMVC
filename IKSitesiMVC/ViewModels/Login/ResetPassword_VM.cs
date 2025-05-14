using System.ComponentModel.DataAnnotations;

namespace IKSitesiMVC.ViewModels.Login
{
    public class ResetPassword_VM
    {
        [Required(ErrorMessage = "E-posta gerekli.")]
        [EmailAddress(ErrorMessage = "Lütfen geçerli bir e-posta adresi giriniz.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Token gereklidir.")]
        public string Token { get; set; }

        [Required(ErrorMessage = "Yeni şifre gerekli.")]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "Şifre tekrarı gerekli.")]
        [Compare("NewPassword", ErrorMessage = "Şifreler eşleşmiyor.")]
        public string ConfirmPassword { get; set; }
    }
}