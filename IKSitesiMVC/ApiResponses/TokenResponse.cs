namespace IKSitesiMVC.ApiResponses
{
    public class TokenResponse
    {
        public string Token { get; set; }
        public DateTime TokenExpiration { get; set; }
        public string Role { get; set; }

        // Her iki durum için ek alanlar
        public int? PersonelID { get; set; }
        public int? KurumID { get; set; }
    }
}