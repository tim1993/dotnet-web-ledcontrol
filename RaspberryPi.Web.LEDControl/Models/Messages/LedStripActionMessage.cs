namespace RaspberryPi.Web.LEDControl.Models.Messages
{
    public class LedStripActionMessage: ILedStripBaseMessage
    {
        public LedStripActions LedStripAction { get; set; }
    }
}
