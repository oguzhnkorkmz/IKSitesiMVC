namespace IKSitesiMVC.Areas.SiteAdmin.DTO_s.Paket
{
    public class PaketDetail_DTO
    {
        public int PaketID { get; set; }
        public string PaketAdi { get; set; }
        public decimal Fiyat { get; set; }
        public int PaketSuresi { get; set; }
        public bool AktifMi { get; set; }
        public int KapasiteSayisi { get; set; }
        public DateTime EklenmeTarihi { get; set; }
        public DateTime? GuncellemeTarihi { get; set; }
        public DateTime? SilmeTarihi { get; set; }
        // KayitDurumu verisini string olarak almayı tercih edebilirsiniz.
        public string KayitDurumu { get; set; }
    }
}
