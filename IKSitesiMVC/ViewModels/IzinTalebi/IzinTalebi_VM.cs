using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace IKSitesiMVC.ViewModels.IzinTalebi
{
    public class IzinTalebi_VM
    {
        [BindNever]
        public int PersonelID { get; set; }

        [Required(ErrorMessage = "Başlangıç tarihi zorunludur.")]
        [DataType(DataType.Date)]
        public DateTime BaslangicTarihi { get; set; }

        [Required(ErrorMessage = "Bitiş tarihi zorunludur.")]
        [DataType(DataType.Date)]
        public DateTime BitisTarihi { get; set; }
    }
}