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

using System;
using System.Windows.Forms;

namespace Ktos.Fa287a
{
    /// <summary>
    /// The main class for running the application
    /// </summary>
    internal class Program
    {
        public const string APPNAME = "FA287A";

        [STAThread]
        private static void Main(string[] args)
        {
            Application.EnableVisualStyles();

            bool result;
            var mutex = new System.Threading.Mutex(true, APPNAME, out result);

            if (!result)
            {
                if (args.Length > 0)
                {
                    if (args[0] == "connect")
                        Ipc.WriteServer(".", APPNAME, (byte)IpcMessages.CONNECT);
                    else if (args[0] == "disconnect")
                        Ipc.WriteServer(".", APPNAME, (byte)IpcMessages.DISCONNECT);
                }
                else
                {
                    MessageBox.Show(Resources.AppResources.AnotherInstanceRunning, Resources.AppResources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                return;
            }

            Application.Run(new ApplicationContext(args));

            GC.KeepAlive(mutex);
        }
    }
}