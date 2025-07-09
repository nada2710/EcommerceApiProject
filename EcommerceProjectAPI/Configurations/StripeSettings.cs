namespace EcommerceProjectAPI.Configurations
{
    public class StripeSettings
    {
        public string SecretKey { get; set; }
        public string PublishableKey { get; set; }
        public string DefaultCurrency { get; set; } = "usd";
    }
}
