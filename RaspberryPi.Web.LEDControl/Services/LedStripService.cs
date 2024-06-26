﻿using Iot.Device.Ft232H;
using Iot.Device.FtCommon;
using Iot.Device.Ws28xx;
using RaspberryPi.Web.LEDControl.Handlers;
using RaspberryPi.Web.LEDControl.Models;
using RaspberryPi.Web.LEDControl.Models.Messages;
using System.Device.Spi;
using System.Drawing;

namespace RaspberryPi.Web.LEDControl.Services
{
    public class LedStripService
    {
        private Task? _knightRiderTask;
        private CancellationTokenSource? _knightRiderCts;
        private Task? _rainbowTask;
        private CancellationTokenSource? _rainbowCts;

        private readonly Ft232HDevice? _ft232hDevice;
        private readonly SpiDevice _spiDevice;
        private readonly SpiConnectionSettings _settings;
        private Ws2812b _ledDevice;
        private int _numberOfLeds;

        public LedStripService(uint numberOfLeds, bool useUsbConverter, SpiConnectionSettings spiConnectionSettings, FtDevice? usbConverterDevice)
        {
            var devices = FtCommon.GetDevices();
            _settings = spiConnectionSettings;

            foreach (var device in devices)
            {
                Console.WriteLine("Printing all detected devices...");
                PrintDeviceInfo(device);
            }

            if (useUsbConverter && usbConverterDevice == null)
            {
                throw new InvalidOperationException("Error: Initialization not possible, FT232H Converter Device must be plugged in via USB.");
            }

            if (useUsbConverter && usbConverterDevice != null)
            {
                // using FT232H for devices without GPIO
                Console.WriteLine("Using detected and configured USB Converter");
                _ft232hDevice = new Ft232HDevice(usbConverterDevice);
                _spiDevice = _ft232hDevice.CreateSpiDevice(_settings);
            }
            else
            {
                // using SpiDevice Create for GPIO of Raspberry Pi
                Console.WriteLine("Using GPIO of Raspberry Pi");
                _spiDevice = SpiDevice.Create(_settings);
            }

            _numberOfLeds = (int)numberOfLeds;
            _ledDevice = new Ws2812b(_spiDevice, _numberOfLeds);
        }

        public void HandleLedStripMessage<T>(T message, CancellationToken cancellationToken) where T : ILedStripBaseMessage
        {
            if (message == null)
            {
                Console.WriteLine("Empty message received");
                return;
            }

            if (_ledDevice == null)
            {
                throw new ArgumentException($"Please instantiate {nameof(LedStripService)} withcorrect SPI Connection Settings");
            }

            switch (message)
            {
                case LedStripResetMessage ledStripResetMessage:
                    _rainbowCts?.Cancel();
                    _knightRiderCts?.Cancel();

                    var img = _ledDevice.Image;
                    img.Clear();
                    _ledDevice.Update();
                    break;
                case LedStripSetLightningMessage lightningMessage:
                    SetPixels(Color.FromArgb(0, lightningMessage.R, lightningMessage.G, lightningMessage.B), lightningMessage.StartIndex, lightningMessage.Length);
                    break;
                case SetLedStripLengthMessage setLedStripLengthMessage:
                    InitializeLedStrip((int)setLedStripLengthMessage.NumberOfLeds);
                    break;
                case LedStripActionMessage ledStripActionMessage:
                    if(ledStripActionMessage.LedStripAction == LedStripActions.KnightRider)
                    {
                        _rainbowCts?.Cancel();

                        _knightRiderCts = new CancellationTokenSource();
                        _knightRiderTask = new Task(async () => await LedStripActionHandlers.KnightRiderAsync(_ledDevice, _numberOfLeds, _knightRiderCts.Token));
                        _knightRiderTask.Start();
                    }

                    if(ledStripActionMessage.LedStripAction == LedStripActions.Rainbow)
                    {
                        _knightRiderCts?.Cancel();

                        _rainbowCts = new CancellationTokenSource();
                        _rainbowTask = new Task(async () => await LedStripActionHandlers.RainbowAsync(_ledDevice, _numberOfLeds, _rainbowCts.Token));
                        _rainbowTask.Start();
                    }
                    break;
                default:
                    throw new NotImplementedException("Message Type not implemented");
            }
        }

        private void InitializeLedStrip(int length)
        {
            _numberOfLeds = length;
            _ledDevice = new Ws2812b(_spiDevice, length);
        }

        private void SetPixels(Color color, int startLEDIndex, int length)
        {
            if (length > _numberOfLeds)
            {
                length = _numberOfLeds;
            }

            Console.WriteLine($"SetPixels: Start LED Index: {startLEDIndex} - Length: {length} - Color: {color}");

            var bitmapImage = _ledDevice.Image;

            for (int i = startLEDIndex; i < startLEDIndex + length; i++)
            {
                bitmapImage.SetPixel(i, 0, color);
            }

            _ledDevice.Update();
        }

        private static void PrintDeviceInfo(FtDevice device)
        {
            Console.WriteLine($"{device.Description}");
            Console.WriteLine($" Flags: {device.Flags}");
            Console.WriteLine($" Id: {device.Id}");
            Console.WriteLine($" LocId: {device.LocId}");
            Console.WriteLine($" Serial number: {device.SerialNumber}");
            Console.WriteLine($" Type: {device.Type}");
        }
    }
}
