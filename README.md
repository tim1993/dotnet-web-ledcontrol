# dotnet-web-ledcontrol

# RaspberryPi Web LEDControl
This project enables you to control a LED strip like Ws2812b over the web.

# Getting started
[Install .NET 6](https://dotnet.microsoft.com/en-us/download/dotnet/6.0/runtime)

You can configure the project using appsettings.json:

```
{
  "HostingSettings": {
    "Port": "<WebServer Port>"
  },
  "LEDStripSettings": {
    "LightUsbId": "<USB-ID>", // e.g. 67330068 of FT232H Breakout Board, to find out start the app and it will print the info
    "NumberOfLeds": <Number-Of-Leds>
  }
}
```

# Build and run
To build and run the application you have to:

Run ```dotnet restore```
then ```dotnet run```

# Links
[FT232H Breakout Board](https://www.adafruit.com/product/2264)