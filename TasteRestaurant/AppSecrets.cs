namespace TasteRestaurant
{
    public class AppSecrets
    {
        public string MailUserName { get; set; }
        public string MailPassword { get; set; }

        public class FacebookApp
        {
            public string AppID { get; set; }
            public string AppSecret { get; set; }
        }

        public class GoogleApp
        {
            public string ClientID { get; set; }
            public string ClientSecret { get; set; }
        }
    }
}
