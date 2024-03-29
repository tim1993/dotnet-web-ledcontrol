using RaspberryPi.Web.LEDControl;
using RaspberryPi.Web.LEDControl.Models.Settings;
using RaspberryPi.Web.LEDControl.Services;

await Host.CreateDefaultBuilder(args)
    .ConfigureWebHostDefaults(webBuilder =>
    {
        var settingsService = new SettingsService();
        LedStripSettings ledStripSettings = settingsService.GetLedStripSettings();
        HostingSettings hostingSettings = settingsService.GetHostingSettings();

        webBuilder.UseStartup<Startup>();
        webBuilder.UseUrls(new[] { $"http://0.0.0.0:{hostingSettings.Port}" });
        webBuilder.ConfigureServices(services =>
        {
            services.AddSingleton(x => new LedStripService(ledStripSettings.LightUsbId, ledStripSettings.NumberOfLeds));
        });
    })
    .Build()
    .RunAsync();

