using PacketDotNet;
using SharpPcap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Курсовой_проект_4_семестр.Models
{
    public class DisplayedPacket
    {

        public int Number { get; private set; }
        public string Time { get; private set; }
        public string Source { get; private set; }
        public string Destination { get; private set;  }
        public string Protocol { get; private set;  }
        public int Length { get; private set;  }

        public Packet Packet { get; private set; }

        public Packet LastPacket { get; private set; }

        public CaptureEventArgs CaptureEvent { get; private set; }

        public DisplayedPacket(int number, PosixTimeval time, string source, string destination, string protocol, int length, Packet packet, Packet lastpacket, CaptureEventArgs captureEvent)
        {
            Number = number;
            Time = time.Date.ToLongTimeString();
            Source = source;
            Destination = destination;
            Protocol = protocol;
            Length = length;
            Packet = packet;
            LastPacket = lastpacket;
            CaptureEvent = captureEvent;
        }
    }
}
