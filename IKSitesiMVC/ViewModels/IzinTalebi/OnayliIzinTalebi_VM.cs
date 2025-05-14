namespace IKSitesiMVC.ViewModels.IzinTalebi
{
    public class OnayliIzinTalebi_VM
    {
        public int IzinTalebiID { get; set; }
        public int PersonelID { get; set; }
        public string PersonelAdi { get; set; }
        public DateTime BaslangicTarihi { get; set; }
        public DateTime BitisTarihi { get; set; }
        public bool Onaylimi { get; set; }

    }
}
