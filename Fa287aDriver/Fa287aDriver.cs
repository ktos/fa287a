#region License

/*
 * FA287A for Windows
 *
 * Copyright (C) Marcin Badurowicz <m at badurowicz dot net> 2016
 *
 *
 * Permission is hereby granted, free of charge, to any person obtaining
 * a copy of this software and associated documentation files
 * (the "Software"), to deal in the Software without restriction,
 * including without limitation the rights to use, copy, modify, merge,
 * publish, distribute, sublicense, and/or sell copies of the Software,
 * and to permit persons to whom the Software is furnished to do so,
 * subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be
 * included in all copies or substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
 * EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
 * OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
 * NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS
 * BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN
 * ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
 * CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 * SOFTWARE.
 */

#endregion License

using System.IO.Ports;

namespace Ktos.Fa287a
{
    /// <summary>
    /// A very basic class for receiving data from the FA287A Bluetooth
    /// keyboard over the Bluetooth Serial Port Profile.
    /// </summary>
    public class Fa287aDriver
    {
        private SerialPort sp;

        /// <summary>
        /// Initializes a new instance and creates a COM port connection
        /// </summary>
        /// <param name="portName">Desired COM port to be used for connection</param>
        public Fa287aDriver(string portName)
        {
            sp = new SerialPort(portName, 9600, Parity.None, 8, StopBits.One);
            sp.DataReceived += Sp_DataReceived;
            sp.ReceivedBytesThreshold = 1;
        }

        /// <summary>
        /// Opens the communication, clears the buffer
        /// </summary>
        public void Open()
        {
            sp.Open();
            sp.ReadExisting();
        }

        /// <summary>
        /// Stops the communication and closes the port
        /// </summary>
        public void Close()
        {
            sp.Close();
        }

        /// <summary>
        /// The event raised when key was released on the keyboard
        /// </summary>
        public event KeyHandler KeyUp;

        /// <summary>
        /// The event raised when key was pressed on the keyboard
        /// </summary>
        public event KeyHandler KeyDown;

        private void Sp_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            while ((sender as SerialPort).BytesToRead > 0)
            {
                var b = (sender as SerialPort).ReadByte();

                // keyboard is sending key code when key is going down
                // and key code + 128 when is going up
                if (b <= 128)
                {
                    KeyDown?.Invoke(this, new KeyHandlerEventArgs((Key)b));
                }
                else
                {
                    KeyUp?.Invoke(this, new KeyHandlerEventArgs((Key)(b - 128)));
                }
            }
        }
    }
}