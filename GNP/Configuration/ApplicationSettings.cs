﻿namespace GNP.Configuration
{
    public class ApplicationSettings
    {
        public EmailConfiguration EmailConfiguration { get; set; }
    }

    public class EmailConfiguration
    {
        public string SmtpServer { get; set; }
        public string From { get; set; }
        public int Port { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}