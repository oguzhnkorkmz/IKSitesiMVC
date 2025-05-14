namespace IKSitesiMVC.ViewModels.HarcamaTalebi
{
    public class OnayliHarcamaTalebi_VM
    {
        public int HarcamaTalebiID { get; set; }
        public int PersonelID { get; set; }
        public string PersonelAdi { get; set; }

        public decimal HarcamaTutari { get; set; }
        public bool Onaylimi { get; set; }
    }
}
