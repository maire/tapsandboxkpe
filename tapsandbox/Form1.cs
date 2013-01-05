using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Win32;
using System.Runtime.InteropServices;


namespace tapsandbox
{
    public partial class Form1 : Form
    {

        List<string> allocatedGUID = new List<string>();
        TapInterface _serverInterface;
        TapInterface _clientInterface;
        byte[] serverLocal = { 10, 4, 0, 1 };
        byte[] serverRemote = { 10, 4, 0, 2 };

        byte[] clientLocal = { 10, 3, 0, 1 };
        byte[] clientRemote = { 10, 3, 0, 2 };

        int clientAddr = 0x0100030a;
        int clientNetwork = 0x0000030a;
        int clientNetmask = 0x00ffffff;

        int serverAddr = 0x0100040a;
        int serverNetwork = 0x0000040a;
        int serverNetmask = 0x00ffffff;

        public Form1()
        {
            InitializeComponent();
            string serverTapGUID = GetDeviceGuid();
            log("Found server TAP " + serverTapGUID + "/" + HumanName(serverTapGUID) + " IP " + humanIP(serverLocal));
            _serverInterface = new TapInterface(serverTapGUID, serverAddr, serverNetwork, serverNetmask);
            string clientTapGUID = GetDeviceGuid();
            log("Found client TAP " + clientTapGUID + "/" + HumanName(clientTapGUID) + " IP " + humanIP(clientLocal));
            _clientInterface = new TapInterface(clientTapGUID, clientAddr, clientNetwork, clientNetmask);
            _serverInterface.NewRead += serverIncoming;
            _clientInterface.NewRead += clientIncoming;
        }

        public void serverIncoming(byte[] packet)
        {
            if (getVersion(packet) == 6)
            {
                log("Dropping IPv6 packet (server)");
                return;
            }
            if (getProto(packet) != 6)
            {
                log("Dropping non-TCP packet (server)");
                return;
            }
            int headerLength = getHeaderLength(packet);
            int sport = getSPORT(packet, headerLength);
            int dport = getDPORT(packet, headerLength);
            byte[] source = getSource(packet);
            byte[] dest = getDest(packet);
            packet = setSource(packet, clientRemote);
            packet = setDest(packet, clientLocal);
            log("Incoming from server interface - " + humanIP(source) + ":" + sport + " to " + humanIP(dest) + ":" + dport);
            _clientInterface.queueWrite(packet);
        }

        public void clientIncoming(byte[] packet)
        {
            if (getVersion(packet) == 6)
            {
                log("Dropping IPv6 packet (client)");
                return;
            }
            if (getProto(packet) != 6)
            {
                log("Dropping non-TCP packet (client)");
                return;
            }
            int headerLength = getHeaderLength(packet);
            int sport = getSPORT(packet, headerLength);
            int dport = getDPORT(packet, headerLength);
            byte[] source = getSource(packet);
            byte[] dest = getDest(packet);
            log("Incoming from client interface - " + humanIP(source) + ":" + sport + " to " + humanIP(dest) + ":" + dport);
            packet = setSource(packet, serverRemote);
            packet = setDest(packet, serverLocal);
            _serverInterface.queueWrite(packet);
        }

        public void log(string line)
        {
            //ReadBox.Text += line + "\r\n";
            //ReadBox.SelectionStart = ReadBox.Text.Length;
            //ReadBox.ScrollToCaret();
            Console.WriteLine(line);
        }

        public static string ByteArrayToString(byte[] ba)
        {
            StringBuilder hex = new StringBuilder(ba.Length * 2);
            foreach (byte b in ba)
                hex.AppendFormat("{0:x2}", b);
            return hex.ToString();
        }

        public string ByteArrayToFriendly(byte[] ba)
        {
            string desc = "SOURCE - ";
            for (int i = 0; i < 4; ++i)
            {
                desc += ba[12 + i] + " ";
            }
            desc += ", DEST - ";
            for (int i = 0; i < 4; ++i)
            {
                desc += ba[16 + i] + " ";
            }
            desc += ", VER - " + getVersion(ba);
            desc += ", HEAD(b) - " + getHeaderLength(ba);
            desc += ", TTL - " + ba[8];
            desc += ", PROTO - " + ba[9];
            desc += ", CHECK - " + ba[10] + ba[11];
            desc += ", TOTAL - " + (ba[2] + ba[3]);
            if (ba[9] == 6)
            {
                desc += ", SPORT - " + (ba[(getHeaderLength(ba) + 0)] << 8 | ba[(getHeaderLength(ba) + 1)]);
                desc += ", DPORT - " + (ba[(getHeaderLength(ba) + 2)] << 8 | ba[(getHeaderLength(ba) + 3)]);
            }
            return desc;
        }

        public int getVersion(byte[] ba)
        {
            BitArray packet = new BitArray(ba);
            int value = 0;
            for (int i = 0; i < 4; i++)
            {
                if (packet[4 + i])
                    value += Convert.ToInt16(Math.Pow(2, i));
            }
            return value;
        }

        public int getProto(byte[] packet)
        {
            return packet[9];
        }

        public byte[] getSource(byte[] packet)
        {
            byte[] source = new byte[4];
            for (int i = 0; i < 4; ++i)
            {
                source[i] += packet[12 + i];
            }
            return source;
        }

        public byte[] getDest(byte[] packet)
        {
            byte[] dest = new byte[4];
            for (int i = 0; i < 4; ++i)
            {
                dest[i] += packet[16 + i];
            }
            return dest;
        }

        public byte[] setSource(byte[] packet, byte[] newSource)
        {
            for (int i = 0; i < 4; ++i)
            {
                packet[12 + i] = newSource[i];
            }
            return packet;
        }

        public byte[] setDest(byte[] packet, byte[] newDest)
        {
            for (int i = 0; i < 4; ++i)
            {
                packet[16 + i] = newDest[i];
            }
            return packet;
        }

        public int getSPORT(byte[] packet, int headerLength)
        {
            //return BitConverter.ToUInt16(new byte[2] { packet[headerLength], packet[headerLength + 1] }, 0);
            return (packet[(headerLength + 0)] << 8 | packet[(headerLength + 1)]);
        }

        public int getDPORT(byte[] packet, int headerLength)
        {
            //return BitConverter.ToUInt16(new byte[2] { packet[headerLength + 2], packet[headerLength + 3] }, 0);
            return (packet[(headerLength + 2)] << 8 | packet[(headerLength + 3)]);
        }

        public int getHeaderLength(byte[] ba)
        {
            BitArray packet = new BitArray(ba);
            int value = 0;
            for (int i = 0; i < 4; i++)
            {
                if (packet[0 + i])
                    value += Convert.ToInt16(Math.Pow(2, i));
            }
            return (value * 32) / 8;
        }

        public int getSourcePort(byte[] ba, int headerLength)
        {
            BitArray packet = new BitArray(ba);
            int value = 0;
            for (int i = 0; i < 16; i++)
            {
                if (packet[i + (headerLength) + 1])
                    value += Convert.ToInt32(Math.Pow(2, i));
            }
            return (value * 32);
        }

        public string humanIP(byte[] ip)
        {
            string ipString = "";
            for (int i = 0; i < 4; i++)
            {
                if (!(ipString == ""))
                    ipString += ".";
                ipString += ip[i];
            }
            return ipString;
        }

        public string GetDeviceGuid()
        {
            const string AdapterKey = "SYSTEM\\CurrentControlSet\\Control\\Class\\{4D36E972-E325-11CE-BFC1-08002BE10318}";
            RegistryKey regAdapters = Registry.LocalMachine.OpenSubKey(AdapterKey, false);
            string[] keyNames = regAdapters.GetSubKeyNames();
            string devGuid = "";
            foreach (string x in keyNames)
            {
                if (x == "Properties")
                    break;
                RegistryKey regAdapter = regAdapters.OpenSubKey(x, false);
                object id = regAdapter.GetValue("ComponentId");
                if (id != null && id.ToString() == "tapstrong" && !allocatedGUID.Contains(regAdapter.GetValue("NetCfgInstanceId").ToString())) devGuid = regAdapter.GetValue("NetCfgInstanceId").ToString();
            }
            allocatedGUID.Add(devGuid);
            return devGuid;
        }

        static string HumanName(string guid)
        {
            const string ConnectionKey = "SYSTEM\\CurrentControlSet\\Control\\Network\\{4D36E972-E325-11CE-BFC1-08002BE10318}";
            if (guid != "")
            {
                RegistryKey regConnection = Registry.LocalMachine.OpenSubKey(ConnectionKey + "\\" + guid + "\\Connection", false);
                object id = regConnection.GetValue("Name");
                if (id != null) return id.ToString();
            }

            return "";
        }
    }
}
