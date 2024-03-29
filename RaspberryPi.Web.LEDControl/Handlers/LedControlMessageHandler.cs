using RaspberryPi.Web.LEDControl.Models.Messages;
using RaspberryPi.Web.LEDControl.Services;
using System.Text.Json;

namespace RaspberryPi.Web.LEDControl.Handlers
{
    public static class LedControlMessageHandler
    {
        public static async Task HandleLedStripMessageAsync<T>(HttpContext context, LedStripService ledStripService, CancellationToken cancellationToken) where T : ILedStripBaseMessage
        {
            await Console.Out.WriteAsync("LED Strip message received");

            var request = context.Request;
            var stream = new StreamReader(request.Body);
            var body = await stream.ReadToEndAsync();

            var parsedMessage = JsonSerializer.Deserialize<T>(body);

            if(parsedMessage != null)
            {
                ledStripService.HandleLedStripMessage(parsedMessage, cancellationToken);
            }
            return;
        }
    }
}
