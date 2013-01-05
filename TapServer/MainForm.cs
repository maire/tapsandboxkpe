using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Diagnostics;

namespace TapServer
{
    public partial class MainForm : Form
    {
        bool nat = true;
        bool netsh = true;
        bool packetLog = true;
        bool mode = false;

        TapInterface tap;

        IPAddress serverLocal;
        IPAddress serverRemote;
        IPAddress clientLocal;
        IPAddress clientRemote;

        delegate void logHandler(string line, string what);

        NatTools tools = new NatTools();

        public MainForm()
        {
            InitializeComponent();
        }

        private void StartButton_Click(object sender, EventArgs e)
        {
            ServerLocalTextbox.Enabled = false;
            ServerRemoteTextbox.Enabled = false;
            ClientLocalTextbox.Enabled = false;
            ClientRemoteTextbox.Enabled = false;
            HostTextbox.Enabled = false;
            StartButton.Enabled = false;
            NatCheckbox.Enabled = false;
            NetshCheckbox.Enabled = false;
            LogCheckbox.Enabled = false;
            ClientModeCheckbox.Enabled = false;
            StartWorker.RunWorkerAsync();
        }

        public void log(string line, string what = "UNDEF")
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new logHandler(log), what, line);
            }
            else
            {
                LogTextbox.Text += String.Format("[{0}] {1}\r\n", line, what);
                LogTextbox.SelectionStart = LogTextbox.Text.Length;
                LogTextbox.ScrollToCaret();
            }
        }

        private void StartWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            // we might not actually need all these values, depending on mode/nat, but whatever
            serverLocal = IPAddress.Parse(ServerLocalTextbox.Text);
            serverRemote = IPAddress.Parse(ServerRemoteTextbox.Text);
            clientLocal = IPAddress.Parse(ClientLocalTextbox.Text);
            clientRemote = IPAddress.Parse(ClientRemoteTextbox.Text);
            // do this correctly later
            tap = new TapInterface(0x0200040a, 0x0000040a, 0x00ffffff);
            log("TAP GUID: " + tap.devGuid);
            log("TAP NAME: " + tap.HumanName());
            tap.NewRead += TapToSocket;
            if (mode)
            {
                StartClientWorker.RunWorkerAsync();
            }
            else
            {
                StartServerWorker.RunWorkerAsync();
            }
        }

        private void StartClientWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            configInterface(tap.HumanName(), clientLocal);
            // sockets
        }

        private void StartServerWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            configInterface(tap.HumanName(), serverLocal);
            // sockets
        }

        public void TapToSocket(byte[] packet)
        {
            // sanitize input, only for tap to socket (for now)
            if (tools.getVersionInt(packet) == 6)
            {
                log("Dropping IPv6 packet", "TAP2SOCK");
                return;
            }
            if (tools.getProtoInt(packet) != 6)
            {
                log("Dropping non-TCP packet", "TAP2SOCK");
                return;
            }
            if (!(tools.getDestination(packet).ToString() == serverRemote.ToString() || tools.getDestination(packet).ToString() == clientRemote.ToString()))
            {
                log("Dropping packet with invalid destination", "TAP2SOCK");
                return;
            }
            // nat will only work in server mode
            if (nat)
            {
                string preNatString = tools.getInfo(packet) + " --";
                if (serverRemote.ToString() == tools.getDestination(packet).ToString())
                {
                    preNatString += "[DNAT]--";
                    packet = tools.setDestination(packet, clientLocal);
                }
                if (serverLocal.ToString() == tools.getSource(packet).ToString())
                {
                    preNatString += "[SNAT]--";
                    packet = tools.setSource(packet, clientRemote);
                }
                log(preNatString + "> " + tools.getInfo(packet), "TAP2SOCK");
            }
            else
            {
                log(tools.getInfo(packet), "TAP2SOCK");
            }
        }

        public void SocketToTap(byte[] packet)
        {
            // nat will only work in server mode
            if (nat)
            {
                string preNatString = tools.getInfo(packet) + " --";
                if (clientLocal.ToString() == tools.getSource(packet).ToString())
                {
                    preNatString += "[SNAT]--";
                    packet = tools.setSource(packet, serverRemote);
                }
                if (clientRemote.ToString() == tools.getDestination(packet).ToString())
                {
                    preNatString += "[DNAT]--";
                    packet = tools.setDestination(packet, serverLocal);
                }
                log(preNatString + "> " + tools.getInfo(packet), "SOCK2TAP");
            }
            else
            {
                log(tools.getInfo(packet), "SOCK2TAP");
            }
            tap.QueueWrite(packet);
        }

        public void configInterface(string tapName, IPAddress staticIP)
        {
            if (!netsh)
            {
                log("ACHTUNG! Netsh auto-setup is disabled. Configure your interface manually!");
                return;
            }
            string netshArgs = String.Format("interface ip set address \"{0}\" static {1} {2}", tapName, staticIP.ToString(), "255.255.255.0");
            log("Running netsh (" + netshArgs + ")");
            Process p = new Process();
            ProcessStartInfo psi = new ProcessStartInfo("netsh", netshArgs);
            p.StartInfo = psi;
            p.Start();
            p.WaitForExit();
            log("Netsh complete.");
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void NatCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            nat = NatCheckbox.Checked;
        }

        private void NetshCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            netsh = NetshCheckbox.Checked;
        }

        private void LogCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            packetLog = LogCheckbox.Checked;
        }

        private void ClientModeCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            if (ClientModeCheckbox.Checked)
            {
                ServerLocalTextbox.Enabled = false;
                ServerRemoteTextbox.Enabled = false;
                NatCheckbox.Enabled = false;
                mode = true;
                nat = false;
            }
            else
            {
                ServerLocalTextbox.Enabled = true;
                ServerRemoteTextbox.Enabled = true;
                NatCheckbox.Enabled = true;
                mode = false;
            }
        }





        
    }
}
