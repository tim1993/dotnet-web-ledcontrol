using RaspberryPi.Web.LEDControl.Handlers;
using RaspberryPi.Web.LEDControl.Models.Messages;
using RaspberryPi.Web.LEDControl.Services;

namespace RaspberryPi.Web.LEDControl
{
    public class Startup
    {
        public void Configure(IApplicationBuilder app)
        {
            var serviceProvider = app.ApplicationServices;

            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                LedStripService ledStripService = serviceProvider.GetService<LedStripService>() ?? throw new InvalidOperationException($"{nameof(LedStripService)} is not injected");

                endpoints.MapGet("/", () => "LED Web Control is running...");
                endpoints.MapPost("/ledcontrol/setlighting", async (context) => await LedControlMessageHandler.HandleLedStripMessageAsync<LedStripSetLightningMessage>(context, ledStripService, default));
                endpoints.MapPost("/ledcontrol/reset", async (context) => await LedControlMessageHandler.HandleLedStripMessageAsync<LedStripResetMessage>(context, ledStripService, default));
                endpoints.MapPost("/ledcontrol/action", async (context) => await LedControlMessageHandler.HandleLedStripMessageAsync<LedStripActionMessage>(context, ledStripService, default));
                endpoints.MapPost("/ledcontrol/setLength", async (context) => await LedControlMessageHandler.HandleLedStripMessageAsync<SetLedStripLengthMessage>(context, ledStripService, default));
            });
        }
    }
}
