using System;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using System.Management;
using System.IO.Ports;

namespace SerialPortsNotifyIcon
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }
        private void MainForm_Shown(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
            Hide();
            listSerialPorts();
        }

        private void listSerialPorts()
        {
            using var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_PnPEntity WHERE Caption like '%(COM%'");
            var portnames = SerialPort.GetPortNames();
            var ports = searcher.Get().Cast<ManagementBaseObject>().ToList().Select(p => p["Caption"].ToString());

            var portList = portnames.Select(n => n + " - " + ports.FirstOrDefault(s => s.Contains('(' + n + ')'))).ToList();

            menuStrip.Items.Clear();
            foreach (string s in portList)
            {
                menuStrip.Items.Add(s, null);
            }
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            listSerialPorts();
        }

    }
}
