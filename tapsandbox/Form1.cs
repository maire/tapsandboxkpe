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

        public delegate void updateTextDelegate(object sender, byte[] packet);

        public Form1()
        {
            InitializeComponent();
            TapInterface _tapInterface = new TapInterface();
            _tapInterface.NewRead += updateText;
        }

        public void updateText(object sender, byte[] packet)
        {
            if (this.InvokeRequired)
                this.BeginInvoke(new updateTextDelegate(updateText), sender, packet);
            else
            {
                ReadBox.Text += (ByteArrayToFriendly(packet) + "\r\n");
                ReadBox.SelectionStart = ReadBox.Text.Length;
                ReadBox.ScrollToCaret();
            }
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
                //none of this works like at all
                desc += ", SPORT1 - " + (ba[(getHeaderLength(ba))] + ba[(getHeaderLength(ba) + 1)]);
                desc += ", DPORT1 - " + (ba[(getHeaderLength(ba) + 2)] + ba[(getHeaderLength(ba) + 3)]);
                desc += ", SPORT2 - " + BitConverter.ToInt16(ba, getHeaderLength(ba));
                desc += ", DPORT2 - " + BitConverter.ToInt16(ba, getHeaderLength(ba) + 2);
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

        public int getHeaderLength(byte[] ba)
        {
            BitArray packet = new BitArray(ba);
            int value = 0;
            for (int i = 0; i < 4; i++)
            {
                if (packet[0 + i])
                    value += Convert.ToInt16(Math.Pow(2, i));
            }
            return (value * 32) /8;
        }

    }
}
