using Iot.Device.FtCommon;
using Microsoft.Extensions.DependencyInjection;
using RaspberryPi.Web.LEDControl;
using RaspberryPi.Web.LEDControl.Models.Settings;
using RaspberryPi.Web.LEDControl.Services;
using System.Device.Gpio;
using System.Device.Spi;

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
            var devices = FtCommon.GetDevices();
            var usbConverterDevice = devices.SingleOrDefault(x => x.Id == Convert.ToUInt32(ledStripSettings.UsbConverterId, 10));
            var usbConverterSpiConnectionSettings = new SpiConnectionSettings(0, 3) { ClockFrequency = 2_400_000, DataBitLength = 8, ChipSelectLineActiveState = PinValue.Low };
            var raspberryGpioSpiConnectionSettings = new SpiConnectionSettings(0, 0) { ClockFrequency = 2_400_000, DataBitLength = 8, ChipSelectLineActiveState = PinValue.Low };
            
            services.AddSingleton(x => new LedStripService(ledStripSettings.NumberOfLeds, ledStripSettings.UseUsbConverter, usbConverterDevice != null ? usbConverterSpiConnectionSettings : raspberryGpioSpiConnectionSettings, usbConverterDevice));
        });
    })
    .Build()
    .RunAsync();

