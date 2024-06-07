namespace GNP.Configuration
{
    public static class Configuration
    {
        public static void ConfigureSession(this IServiceCollection services)
        {
            // Add session services
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30); // Set the session timeout
                options.Cookie.HttpOnly = true; // Set the cookie to HTTP only
                options.Cookie.IsEssential = true; // Mark the session cookie as essential
            });

            // Add MVC services to the service collection
            services.AddControllersWithViews();
        }
    }
}
