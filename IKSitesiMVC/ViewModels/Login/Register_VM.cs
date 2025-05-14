using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace IKSitesiMVC.ViewModels.Login
{
    public class Register_VM
    {
        [Required(ErrorMessage = "Kullanıcı adı gereklidir.")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Email gereklidir.")]
        [EmailAddress(ErrorMessage = "Geçerli bir email girin.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Şifre gereklidir.")]
        [DataType(DataType.Password)]
        public string Sifre { get; set; }

        [Required(ErrorMessage = "Şifre tekrarı gereklidir.")]
        [DataType(DataType.Password)]
        [Compare("Sifre", ErrorMessage = "Şifreler uyuşmuyor.")]
        public string SifreOnayi { get; set; }

        [Required(ErrorMessage = "Vergi numarası gereklidir.")]
        public string VergiNumarasi { get; set; }

        [Required(ErrorMessage = "Kurum adı gereklidir.")]
        public string KurumAdi { get; set; }

        [Required(ErrorMessage = "Adres gereklidir.")]
        public string Adres { get; set; }

        [Required(ErrorMessage = "Paket seçimi gereklidir.")]
        public int PaketID { get; set; } // Seçilen Paket ID

        public List<SelectListItem> Paketler { get; set; } = new List<SelectListItem>(); // Dropdown için
    }
}
