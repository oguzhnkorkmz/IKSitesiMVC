namespace IKSitesiMVC.Areas.SirketAdmin.ViewModels.Talep
{
    public class HarcamaTalebiList_VM
    {
        public int HarcamaTalebiID { get; set; }
        public int PersonelID { get; set; }
        public string PersonelAdi { get; set; } // UI için ek bilgi (örneğin, personelin adı soyadı)

        public decimal HarcamaTutari { get; set; }
        public string Aciklama { get; set; }
        public bool Onaylimi { get; set; }
        public int KurumID { get; set; }
    }
}
