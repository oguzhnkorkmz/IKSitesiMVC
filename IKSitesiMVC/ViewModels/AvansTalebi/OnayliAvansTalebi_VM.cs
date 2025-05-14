namespace IKSitesiMVC.ViewModels.AvansTalebi
{
    public class OnayliAvansTalebi_VM
    {
        public int AvansTalebiID { get; set; }
        public int PersonelID { get; set; }
        public string PersonelAdi { get; set; }

        public decimal TalepEdilenTutar { get; set; }
        public bool Onaylimi { get; set; }
    }
}
