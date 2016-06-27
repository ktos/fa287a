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

using Microsoft.Win32;
using System;
using System.Configuration;
using System.IO;
using System.Windows.Forms;

namespace Ktos.Fa287a
{
    /// <summary>
    /// Application Context - main class for handling user interface
    /// </summary>
    internal class ApplicationContext : System.Windows.Forms.ApplicationContext
    {
        private KeyboardSimulator ks;
        private NotifyIcon trayIcon;
        private bool connected;

        /// <summary>
        /// Creates a new tray icon and menu for user interaction
        /// </summary>
        public ApplicationContext(string[] args)
        {
            trayIcon = new NotifyIcon();
            trayIcon.Icon = Resources.AppResources.icon;

            trayIcon.DoubleClick += connect;

            trayIcon.ContextMenu = new ContextMenu();
            trayIcon.ContextMenu.MenuItems.Add(new MenuItem(string.Format("&{0}", Resources.AppResources.Connect), connect));
            trayIcon.ContextMenu.MenuItems.Add(new MenuItem(string.Format("&{0}", Resources.AppResources.Disconnect), disconnect));
            trayIcon.ContextMenu.MenuItems.Add(new MenuItem("-"));
            trayIcon.ContextMenu.MenuItems.Add(new MenuItem(string.Format("&{0}", Resources.AppResources.About), about));
            trayIcon.ContextMenu.MenuItems.Add(new MenuItem("-"));
            trayIcon.ContextMenu.MenuItems.Add(new MenuItem(string.Format("&{0}", Resources.AppResources.Exit), exit));
            updateTitle();
            trayIcon.Visible = true;

            ks = new KeyboardSimulator(ConfigurationManager.AppSettings["portName"]);

            // listing to power state changes and removing the temporary file if exists
            SystemEvents.PowerModeChanged += SystemEvents_PowerModeChanged;
            if (File.Exists(Path.Combine(Path.GetTempPath(), "Fa287active.tmp")))
                File.Delete(Path.Combine(Path.GetTempPath(), "Fa287active.tmp"));

            if (args.Length > 0)
                if (args[0] == "connect") connect(this, null);
        }

        private void about(object sender, EventArgs e)
        {
            MessageBox.Show(Resources.AppResources.AboutBox.Replace("\\n", "\n"), Resources.AppResources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void updateTitle()
        {
            trayIcon.Text = connected ?
                    string.Format("{0} - {1} ({2})", Resources.AppResources.AppName, Resources.AppResources.Connected, ConfigurationManager.AppSettings["portName"])
                :
                    string.Format("{0} ({1})", Resources.AppResources.AppName, ConfigurationManager.AppSettings["portName"]);
        }

        private void exit(object sender, EventArgs e)
        {
            trayIcon.Visible = false;            
            Application.Exit();
        }

        private void disconnect(object sender, EventArgs e)
        {
            ks.Close();
            connected = false;
            updateTitle();
        }

        private void connect(object sender, EventArgs e)
        {
            try
            {
                ks.Open();
                connected = true;
                updateTitle();
            }
            catch (IOException)
            {
                trayIcon.ShowBalloonTip(1000, Resources.AppResources.AppName, Resources.AppResources.CannotConnect, ToolTipIcon.Error);
            }
        }

        /// <summary>
        /// Responds to system power mode changes: going to sleep and resuming. If system is going to sleep, disconnects the keyboard,
        /// but saves information there was a connection active, connection is being resumed after power resume
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SystemEvents_PowerModeChanged(object sender, PowerModeChangedEventArgs e)
        {
            if (e.Mode == PowerModes.Suspend)
            {
                if (ks.IsOpen)
                {
                    File.Create(Path.Combine(Path.GetTempPath(), "Fa287active.tmp"));
                    ks.Close();
                }
            }

            if (e.Mode == PowerModes.Resume)
            {
                if (File.Exists(Path.Combine(Path.GetTempPath(), "Fa287active.tmp")))
                {
                    if (Convert.ToBoolean(ConfigurationManager.AppSettings["autoResume"]))
                        ks.Open();

                    File.Delete(Path.Combine(Path.GetTempPath(), "Fa287active.tmp"));
                }
            }
        }
    }
}