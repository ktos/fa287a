using System;
using System.Configuration;
using System.IO;
using System.Windows.Forms;

namespace Ktos.Fa287a
{
    internal class ApplicationContext : System.Windows.Forms.ApplicationContext
    {
        private KeyboardSimulator ks;
        private NotifyIcon trayIcon;
        private bool IsConnected;

        public ApplicationContext()
        {
            trayIcon = new NotifyIcon();
            trayIcon.Icon = Resources.AppResources.icon;

            trayIcon.DoubleClick += connect;

            trayIcon.ContextMenu = new ContextMenu();
            trayIcon.ContextMenu.MenuItems.Add(new MenuItem(string.Format("&{0}", Resources.AppResources.Connect), connect));
            trayIcon.ContextMenu.MenuItems.Add(new MenuItem(string.Format("&{0}", Resources.AppResources.Disconnect), disconnect));
            trayIcon.ContextMenu.MenuItems.Add(new MenuItem("-"));
            trayIcon.ContextMenu.MenuItems.Add(new MenuItem(string.Format("&{0}", Resources.AppResources.Exit), exit));
            updateTitle();
            trayIcon.Visible = true;

            ks = new KeyboardSimulator(ConfigurationManager.AppSettings["portName"]);
        }

        private void updateTitle()
        {
            trayIcon.Text = IsConnected ? 
                string.Format("{0} - {1} ({2})", Resources.AppResources.AppName, Resources.AppResources.Connected,
                ConfigurationManager.AppSettings["portName"]) : string.Format("{0} ({1})", Resources.AppResources.AppName, 
                ConfigurationManager.AppSettings["portName"]);
        }

        private void exit(object sender, EventArgs e)
        {
            // We must manually tidy up and remove the icon before we
            // exit. Otherwise it will be left behind until the user
            // mouses over.
            trayIcon.Visible = false;
            Application.Exit();
        }

        private void disconnect(object sender, EventArgs e)
        {
            ks.Close();
            IsConnected = false;
            updateTitle();
        }

        private void connect(object sender, EventArgs e)
        {
            try
            {
                ks.Open();
                IsConnected = true;
                updateTitle();
            }
            catch (IOException)
            {
                trayIcon.ShowBalloonTip(1000, Resources.AppResources.AppName, Resources.AppResources.CannotConnect, ToolTipIcon.Error);
            }
        }
    }
}