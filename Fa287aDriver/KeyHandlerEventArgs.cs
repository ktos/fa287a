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

namespace Ktos.Fa287a
{
    /// <summary>
    /// A function for receiving key up or down events from Bluetooth keyboard
    /// </summary>
    /// <param name="sender">Object which sends the event</param>
    /// <param name="e">An object defining key which was pressed</param>
    public delegate void KeyHandler(object sender, KeyHandlerEventArgs e);

    /// <summary>
    /// The class for encapsulating the key which was going up or down on the
    /// FA287A device
    /// </summary>
    public class KeyHandlerEventArgs
    {
        /// <summary>
        /// Initializes a new instance and creates a readonly
        /// field for key pressed.
        /// </summary>
        /// <param name="key"></param>
        public KeyHandlerEventArgs(Key key)
        {
            Key = key;
        }

        /// <summary>
        /// The key which was pressed on the device
        /// </summary>
        public Key Key { get; private set; }
    }
}