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
using System.Net;
using Microsoft.Win32;
using System.Runtime.InteropServices;

namespace TapServer
{
    class NatTools
    {
        // get version as int from IP header
        public int getVersionInt(byte[] ba)
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

        // get protocol as int from IP header
        public int getProtoInt(byte[] packet)
        {
            return packet[9];
        }

        // get source address from IP header
        public IPAddress getSource(byte[] packet)
        {
            byte[] source = new byte[4];
            for (int i = 0; i < 4; ++i)
            {
                source[i] += packet[12 + i];
            }
            return (new IPAddress(source));
        }

        // get destination address from IP header
        public IPAddress getDestination(byte[] packet)
        {
            byte[] dest = new byte[4];
            for (int i = 0; i < 4; ++i)
            {
                dest[i] += packet[16 + i];
            }
            return (new IPAddress(dest));
        }

        // set source address in IP header
        public byte[] setSource(byte[] packet, IPAddress newSourceObj)
        {
            byte[] newSource = newSourceObj.GetAddressBytes();
            for (int i = 0; i < 4; ++i)
            {
                packet[12 + i] = newSource[i];
            }
            return packet;
        }

        // set destination address in IP header
        public byte[] setDestination(byte[] packet, IPAddress newDestObj)
        {
            byte[] newDest = newDestObj.GetAddressBytes();
            for (int i = 0; i < 4; ++i)
            {
                packet[16 + i] = newDest[i];
            }
            return packet;
        }

        // get source port int from TCP header
        public int getSourcePortInt(byte[] packet, int headerLength)
        {
            return (packet[(headerLength + 0)] << 8 | packet[(headerLength + 1)]);
        }

        // get destination port int from TCP header
        public int getDestinationPortInt(byte[] packet, int headerLength)
        {
            return (packet[(headerLength + 2)] << 8 | packet[(headerLength + 3)]);
        }

        // get IP header length int, in quantity of bytes
        public int getHeaderLengthInt(byte[] ba)
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

        // get a string describing the packet
        public string getInfo(byte[] packet)
        {
            string desc = "";
            int headerLength = getHeaderLengthInt(packet);
            desc += String.Format("{0}:{1} (to) {2}:{3}", getSource(packet).ToString(), getSourcePortInt(packet, headerLength), getDestination(packet).ToString(), getDestinationPortInt(packet, headerLength));
            return desc;
        }
    }
}
