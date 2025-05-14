using System.ComponentModel.DataAnnotations;

namespace IKSitesiMVC.ViewModels.Login
{
    public class ForgotPassword_VM
    {
        [Required(ErrorMessage = "Lütfen e-posta adresinizi giriniz.")]
        [EmailAddress(ErrorMessage = "Lütfen geçerli bir e-posta adresi giriniz.")]
        public string Email { get; set; }
    }
}