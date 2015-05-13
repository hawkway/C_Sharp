using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Management.Automation;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Net.NetworkInformation;
using System.Threading.Tasks;

/* I had to learn C# before starting a job, and a friend said that he could use an application that displayed network info. 
 * I used 1.5 functions from StackOverflow to assemble my first C# project */

//---------------------------------------------------------------------------------------------------

namespace SystemInfo {

    class InfoClass
    {

        // power shell object for executing commands
        private PowerShell powerShell = PowerShell.Create();
        
        // power shell script to get the bios serial number
        private const string BIOS_SERIAL = "(get-wmiobject win32_bios).SerialNumber";

        //---------------------------------------------------------------------------------------------------
        // default constructor that prints information to the console for the user

        public InfoClass() {
            Console.WriteLine("Machine Name: " + getMachineName());
            Console.WriteLine("");
            addScript(BIOS_SERIAL);
            executeScript();
            Console.WriteLine("");
            Console.WriteLine("Ethernet:");
            Console.WriteLine("  IPv4: " + getEthernetIP());
            Console.WriteLine("  MAC: " + getEthernetMAC());
            Console.WriteLine("");
            Console.WriteLine("Wireless: ");
            Console.WriteLine("  IPv4: " + getWirelessIP());
            Console.WriteLine("  MAC: " + getWirelessMAC());
            Console.WriteLine("");
            Console.ReadLine();
        } // end constructor

        //---------------------------------------------------------------------------------------------------
        // get the local IP for the network device of given type
        // found on stackOverflow

        public string GetLocalIPv4(NetworkInterfaceType _type) {
            string output = "";
            foreach (NetworkInterface item in NetworkInterface.GetAllNetworkInterfaces()) {
                if (item.NetworkInterfaceType == _type && item.OperationalStatus == OperationalStatus.Up) {
                    foreach (UnicastIPAddressInformation ip in item.GetIPProperties().UnicastAddresses) {
                        if (ip.Address.AddressFamily == AddressFamily.InterNetwork) {
                            output = ip.Address.ToString();
                        } // end if
                    } // end foreach
                } // end if
            } // end foreach
            return output;
        } // end GetLocalIPv4

        //---------------------------------------------------------------------------------------------------
        // modified previous function to get the physical address of the device in question

        public string GetDeviceMACAddrress(NetworkInterfaceType _type)
        {
            string output = "";
            foreach (NetworkInterface item in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (item.NetworkInterfaceType == _type && item.OperationalStatus == OperationalStatus.Up)
                {
                    output = item.GetPhysicalAddress().ToString();
                }
            }
            return output;
        }

        //---------------------------------------------------------------------------------------------------
        // getter for system name

        public string getMachineName()
        {
            return System.Environment.MachineName;
        } // end getMachineName

        //---------------------------------------------------------------------------------------------------
        // getter for ethernet device IP

        public string getEthernetIP()
        {
            return GetLocalIPv4(NetworkInterfaceType.Ethernet);
        } // end getEthernetIP

        //---------------------------------------------------------------------------------------------------
        // getter for ethernet mac address

        public string getEthernetMAC()
        {
            return parseAddress(GetDeviceMACAddrress(NetworkInterfaceType.Ethernet));
        } // end getEthernetMAC

        //---------------------------------------------------------------------------------------------------
        // getter for wireless device IP

        public string getWirelessIP()
        {
            return GetLocalIPv4(NetworkInterfaceType.Wireless80211);
        } // end getWirelessIP

        //---------------------------------------------------------------------------------------------------
        // getter for wireless mac address

        public string getWirelessMAC()
        {
            return parseAddress(GetDeviceMACAddrress(NetworkInterfaceType.Wireless80211));
        } // end getWirelessMAC

        //---------------------------------------------------------------------------------------------------
        // add a colon delimiter to the mac address

        private string parseAddress(string s)
        {
            return String.Join(":", Regex.Matches(s, @"(\d|\D){2}").Cast<Match>());
        } // end parseAddress

        //---------------------------------------------------------------------------------------------------
        // add a power shell command to the object

        public void addScript(string myScript)
        {
            powerShell.AddScript(myScript);
        } // end addScript

        //---------------------------------------------------------------------------------------------------
        // execute the previously added power shell command(s)

        public void executeScript()
        {
            Collection<PSObject> psOutput = powerShell.Invoke();
            foreach (PSObject item in psOutput)
            {
                Console.WriteLine("BIOS Serial: " + item.BaseObject.ToString());
            } // end foreach
        } // end executeScript

        //---------------------------------------------------------------------------------------------------
        // write given text to file on desktop

        private void writeToFile(string myText)
        {
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string filePath = desktopPath + "\\SystemInfo.txt";
            File.WriteAllText(@filePath, myText);
        }

        //---------------------------------------------------------------------------------------------------
        // write given text to specified path

        private void writeToFile(string myPath, string myText)
        {
            File.WriteAllText(myPath, myText);
        } // end overloaded

        //---------------------------------------------------------------------------------------------------

    } // end class

} // end namespace

