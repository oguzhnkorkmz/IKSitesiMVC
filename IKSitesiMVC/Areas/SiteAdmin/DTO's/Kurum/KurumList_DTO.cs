namespace IKSitesiMVC.Areas.SiteAdmin.DTO_s
{
    public class KurumList_DTO
    {
        public int KurumID { get; set; }
        public string KurumAdi { get; set; }
        public DateTime PaketBaslangicTarihi { get; set; }
        public DateTime PaketBitisTarihi { get; set; }
        public bool PaketAktifMi { get; set; }

    }
}
