namespace IKSitesiMVC.Areas.SirketAdmin.ViewModels.Talep
{
    public class IzinTalebiList_VM
    {
        public int IzinTalebiID { get; set; }
        public int PersonelID { get; set; }
        public string PersonelAdi { get; set; } // UI için ek bilgi (örneğin, personelin adı soyadı)
        public DateTime BaslangicTarihi { get; set; }
        public DateTime BitisTarihi { get; set; }
        public bool Onaylimi { get; set; }
        public int KurumID { get; set; }
    }
}
