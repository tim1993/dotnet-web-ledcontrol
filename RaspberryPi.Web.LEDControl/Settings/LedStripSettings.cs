
namespace RaspberryPi.Web.LEDControl.Models.Settings
{
    public class LedStripSettings
    {
        public uint NumberOfLeds { get; set; } = 0;

        public string UsbConverterId { get; set; } = "0";

        public bool UseUsbConverter { get; set; } = false;
    }
}
