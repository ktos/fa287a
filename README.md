FA287A for Windows
==================

[![License](https://img.shields.io/badge/license-MIT-blue.svg)](https://raw.githubusercontent.com/ktos/DjToKey/devel/LICENSE)
[![GitHub release](https://img.shields.io/github/release/ktos/fa287a.svg)]()

A simple applications handling connection with your HP FA287A Bluetooth keyboard
so it works in regular Windows desktop.

## Short description

Old HP Bluetooth keyboard, model FA287A, does not work over HID Bluetooth profile,
so it is not supported as a keyboard in Windows. This small application handles
communication with the keyboard over the serial port and simulates keypresses
for the system when keypress is sent from the keyboard.

The keyboard has a set of additional keys (Mail, Calendar, Tasks, etc.), but they
are not supported at the moment.

## Configuration

The configuration is handled by `Fa287a.exe.config` file, where you need to set
the `value` property for key `portName` in `appSettings`, for example:

```xml
  <appSettings>
    <add key="portName" value="COM5" />
  </appSettings>
```

After pairing with this keyboard you have two COM ports created (check Bluetooth
settings applet in Control Panel) - you need to put there the name for **outgoing**
port, named `BT-FoldableKB 'SPP slave'` usually.

## Basic usage

After starting the application it resides in your system tray. Right clicking its
tray icon will give you a context menu with options for connecting to the keyboard
or disconnecting (or closing the application). Double-clicking the icon also starts
the connection. Remember that keyboard must be active (orange LED blinking) before
connecting, the LED will be turned off when keyboard will connect to the application.

Application is written in C# for the .NET 4.5.2 platform, so you have to have it
installed.

## Contributing

You are welcome to contribute to DjToKey! Create issues, Pull Requests, fork it, 
change it, remix it, as long as you confirm to the [MIT license](LICENSE).

Have a lot of fun.  
-- ktos