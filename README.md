# dotnet-web-ledcontrol

# RaspberryPi Web LEDControl
This project enables you to control a LED stripe like Ws2812b over the web using a converter like FT232H breakout board or GPIO of Raspberry Pi.

# Getting started
[Install .NET 8](https://dotnet.microsoft.com/en-us/download/dotnet/8.0/runtime)

You can configure the project using appsettings.json:

```
{
  "HostingSettings": {
    "Port": "<WebServer Port>"
  },
  "LEDStripSettings": {
    "UseUsbConverter": <true/false>, // Set to True when FT232H is used, set to false when Raspberry Pi GPIO is used
    "UsbConverterId": <USB-ID>, // Set to USB Converter ID which is printed out on app start, set to null when Raspberry Pi GPIO is used
    "NumberOfLeds": <Number-Of-Leds> // Number of LEDs of your stripe
  }
}
```

# Build and run
To build and run the application you have to:

Run ```dotnet restore```
then ```dotnet run```

# API

API Method                      | Body                                                                                                                                                               | Action                                                                             |
--------------------------------|--------------------------------------------------------------------------------------------------------------------------------------------------------------------|------------------------------------------------------------------------------------|
GET /                           |                                                                                                                                                                    | Returns 'LED Web Control is running...' message                                    |
POST /ledcontrol/setlighting    | ```{ "R": <Red-Value>, "G": <Green-Value>, "B": <Blue-Value>, "StartIndex": <Start-Index>, "Length": <Length> }```                                                 | Sets the segments of LED stripe from start index to length with given RGB-Value    |
POST /ledcontrol/reset          | ```{ }```                                                                                                                                                          | Resets the LED stripe and stops all started actions                                |
POST /ledcontrol/action         | ``` { "LedStripAction": <LED-StripAction = 1 -> KnightRider, 2 -> Rainbow LED> }  ```                                                                              | Enables a cool LED sequence                                                        |
POST /ledcontrol/setLength      | ``` { "NumberOfLeds": 200 } ```                                                                                                                                    | Overwrite LED stripe length during runtime                                         |

# Links
[FT232H Breakout Board](https://www.adafruit.com/product/2264)