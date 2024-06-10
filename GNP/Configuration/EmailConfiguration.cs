namespace GNP.Configuration
{
    public class EmailConfiguration
    {
        public string From { get; set; }
        public string SmtpServer { get; set; }
        public int Port { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }

       
    }

    public static class EmailExtension
    {
        public static void ConfigureMailService(this IServiceCollection services, IConfiguration Configuration)
        {
            var emailConfig = Configuration
               .GetSection("EmailConfiguration")
               .Get<EmailConfiguration>();
            services.AddSingleton(emailConfig);
        }
    }

}
