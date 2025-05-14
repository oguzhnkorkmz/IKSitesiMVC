namespace IKSitesiMVC.Areas.SiteAdmin.DTO_s
{
    public class KurumDetay_DTO
    {
        public int KurumID { get; set; }
        public string KurumAdi { get; set; }
        public string Adres { get; set; }
        public int PaketID { get; set; }

        // Eğer paketle ilgili daha detaylı bilgi göstermek isterseniz,
        // örneğin paket adını veya özet bilgisini tutmak için bir alan.
        public string PaketBilgi { get; set; }

        public DateTime PaketBaslangicTarihi { get; set; }
        public DateTime PaketBitisTarihi { get; set; }
        public bool PaketAktifMi { get; set; }
        public string VergiNumarasi { get; set; }
        public DateTime EklenmeTarihi { get; set; }
        public DateTime? GuncellemeTarihi { get; set; }
        public DateTime? SilmeTarihi { get; set; }

        // Enum değerini okunabilir string olarak göstermek avantajlıdır.
        public string KayitDurumu { get; set; }

    }
}
