using RaspberryPi.Web.LEDControl.Models.Settings;

namespace RaspberryPi.Web.LEDControl.Services
{
    internal class SettingsService
    {
        private readonly IConfiguration _configuration;

        public SettingsService() 
        {
            _configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").AddEnvironmentVariables().Build();
        }

        public LedStripSettings GetLedStripSettings()
        {
            var ledStripSettings = _configuration.GetRequiredSection("LEDStripSettings").Get<LedStripSettings>();
            return ledStripSettings ?? throw new InvalidOperationException("LEDStripSettings couldn't be loaded");
        }

        public HostingSettings GetHostingSettings()
        {
            var hostingSettings = _configuration.GetRequiredSection("HostingSettings").Get<HostingSettings>();
            return hostingSettings ?? throw new InvalidOperationException("HostingSettings couldn't be loaded");
        } 
    }
}
