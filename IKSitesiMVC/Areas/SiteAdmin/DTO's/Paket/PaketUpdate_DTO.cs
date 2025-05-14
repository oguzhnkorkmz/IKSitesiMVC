namespace IKSitesiMVC.Areas.SiteAdmin.DTO_s.Paket
{
    public class PaketUpdate_DTO
    {
        public int PaketID { get; set; }
        public string PaketAdi { get; set; }
        public decimal Fiyat { get; set; }
        public int PaketSuresi { get; set; }
        public bool AktifMi { get; set; }
        public int KapasiteSayisi { get; set; }
    }
}
