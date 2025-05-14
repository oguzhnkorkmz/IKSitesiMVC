using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace IKSitesiMVC.Areas.SirketAdmin.ViewModels.Personel
{
    public class PersonelEkle_VM
    {
        [Required(ErrorMessage = "Personel adı zorunludur.")]
        public string PersonelAdi { get; set; }

        [Required(ErrorMessage = "Personel soyadı zorunludur.")]
        public string PersonelSoyadi { get; set; }

        [Required(ErrorMessage = "Email zorunludur.")]
        [EmailAddress(ErrorMessage = "Geçerli bir email giriniz.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Şifre zorunludur.")]
        [DataType(DataType.Password)]
        public string Sifre { get; set; }

        [Required(ErrorMessage = "Şifre onayı zorunludur.")]
        [DataType(DataType.Password)]
        [Compare("Sifre", ErrorMessage = "Şifreler uyuşmuyor.")]
        [Display(Name = "Şifre (Tekrar)")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Bölüm seçimi zorunludur.")]
        [Display(Name = "Bölüm")]
        public int BolumID { get; set; }

        // Bu alanı formdan aldırmıyor, sadece dropdown için. Doğrulamada atla:
        [ValidateNever]
        public List<SelectListItem> Bolumler { get; set; } = new List<SelectListItem>();
    }
}
