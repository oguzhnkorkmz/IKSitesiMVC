namespace IKSitesiMVC.Areas.SirketAdmin.ViewModels.Talep
{
    public class AvansTalebiList_VM
    {
        public int AvansTalebiID { get; set; }
        public int PersonelID { get; set; }
        public string PersonelAdi { get; set; } // UI için ek bilgi (örneğin, personelin adı soyadı)

        public decimal TalepEdilenTutar { get; set; }
        public bool Onaylimi { get; set; }
        public int KurumID { get; set; }
    }
}
