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

        public ApplicationContext()
        {
            trayIcon = new NotifyIcon();
            trayIcon.Icon = Resources.AppResources.icon;

            trayIcon.DoubleClick += connect;

            trayIcon.ContextMenu = new ContextMenu();
            trayIcon.ContextMenu.MenuItems.Add(new MenuItem("&Connect", connect));
            trayIcon.ContextMenu.MenuItems.Add(new MenuItem("&Disconnect", disconnect));
            trayIcon.ContextMenu.MenuItems.Add(new MenuItem("-"));
            trayIcon.ContextMenu.MenuItems.Add(new MenuItem("&Exit", exit));
            trayIcon.Text = "Fa287a " + ConfigurationManager.AppSettings["portName"];
            trayIcon.Visible = true;

            ks = new KeyboardSimulator(ConfigurationManager.AppSettings["portName"]);
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
        }

        private void connect(object sender, EventArgs e)
        {
            try
            {
                ks.Open();
            }
            catch (IOException)
            {
                trayIcon.ShowBalloonTip(1000, "Fa287a", "Cannot connect to keyboard. Check if the port is correct and if keyboard is active.", ToolTipIcon.Error);
            }
        }
    }
}