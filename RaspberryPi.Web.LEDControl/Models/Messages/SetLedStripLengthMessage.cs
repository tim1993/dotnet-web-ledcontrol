namespace RaspberryPi.Web.LEDControl.Models.Messages
{
    public class SetLedStripLengthMessage: ILedStripBaseMessage
    {
        public uint NumberOfLeds { get; set; } = 0;
    }
}
