using System;
using System.Drawing;
using System.Windows.Forms;

/* simple system tray layout found somewhere on the internet, and modified by me */

namespace SystemInfoTrayApp
{
    public class SystemInfoTrayApp : Form
    {
        [STAThread]
        public static void Main()
        {
            Application.Run(new SystemInfoTrayApp());
        }

        private NotifyIcon trayIcon;
        private ContextMenu trayMenu;

        public SystemInfoTrayApp()
        {

            // new class instance that gets system information
            InfoClass sysInfo = new InfoClass();

            // create the main tray menu
            trayMenu = new ContextMenu();
            
            // create the line items of the main menu
            MenuItem computerName = new MenuItem("System Name");
            MenuItem ethernetInfo = new MenuItem("Ethernet");
            MenuItem wirelessInfo = new MenuItem("Wireless");
            MenuItem biosSerial = new MenuItem("BIOS Serial");
            // submenu for machine name
            computerName.MenuItems.Add(new MenuItem(sysInfo.getMachineName()));
            // ethernet adapter ip and address info
            ethernetInfo.MenuItems.Add(new MenuItem(sysInfo.getEthernetIP()));
            ethernetInfo.MenuItems.Add(new MenuItem(sysInfo.getEthernetMAC()));
            // wireless adapter ip and address info
            wirelessInfo.MenuItems.Add(new MenuItem(sysInfo.getWirelessIP()));
            wirelessInfo.MenuItems.Add(new MenuItem(sysInfo.getWirelessMAC()));
            // bios serial number
            biosSerial.MenuItems.Add(new MenuItem(sysInfo.getBiosSerial()));
            
            // add items to main menu
            trayMenu.MenuItems.Add(computerName);
            trayMenu.MenuItems.Add(biosSerial);

            // don't add adapter if missing
            if (sysInfo.isDeviceAlive("ethernet")) {
                trayMenu.MenuItems.Add(ethernetInfo);
            } // end if
            
            // dont add adapter if missing
            if (sysInfo.isDeviceAlive("wireless")) {
                trayMenu.MenuItems.Add(wirelessInfo);
            } // end if
            
            // exit the application
            trayMenu.MenuItems.Add("Exit", OnExit);

            // Create a tray icon. In this example we use a
            // standard system icon for simplicity, but you
            // can of course use your own custom icon too.
            trayIcon = new NotifyIcon();
            trayIcon.Text = "SysTrayInfoApp";
            trayIcon.Icon = new Icon(SystemIcons.Application, 40, 40);

            // Add menu to tray icon and show it.
            trayIcon.ContextMenu = trayMenu;
            trayIcon.Visible = true;
        }

        protected override void OnLoad(EventArgs e)
        {
            Visible = false; // Hide form window.
            ShowInTaskbar = false; // Remove from taskbar.

            base.OnLoad(e);
        }

        private void OnExit(object sender, EventArgs e)
        {
            Application.Exit();
        }

        protected override void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {
                // Release the icon resource.
                trayIcon.Dispose();
            }

            base.Dispose(isDisposing);
        }
    }
}