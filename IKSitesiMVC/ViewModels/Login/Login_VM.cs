using System.ComponentModel.DataAnnotations;

namespace IKSitesiMVC.ViewModels.Login
{
    public class Login_VM
    {
        [Required] public string Email { get; set; }
        [Required] public string Sifre { get; set; }
        [Required] public string GirisTipi { get; set; }  // yeni
    }
}
