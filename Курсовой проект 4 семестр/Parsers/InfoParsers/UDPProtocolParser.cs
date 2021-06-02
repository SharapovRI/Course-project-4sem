using PacketDotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using Курсовой_проект_4_семестр.Models;

namespace Курсовой_проект_4_семестр.Parsers.InfoParsers
{
    public class UDPProtocolParser
    {
        public static Expander GetTCPPacketInfo(DisplayedPacket packet)
        {
            if (packet is null) return null;

            Expander udpExpander = new Expander();
            StackPanel stackPanel = new StackPanel();
            udpExpander.Content = stackPanel;

            UdpPacket udpPacket = packet.Packet.Extract<UdpPacket>();
            if (udpPacket is null) return null;

            string sourcePort = udpPacket.SourcePort.ToString();
            stackPanel.Children.Add(new TextBlock { Text = "Source Port: " + sourcePort });

            string destPort = udpPacket.DestinationPort.ToString();
            stackPanel.Children.Add(new TextBlock { Text = "Destination Port: " + destPort });

            string len = udpPacket.Length.ToString();
            stackPanel.Children.Add(new TextBlock { Text = "Length: " + len });

            string checksum = Convert.ToString(udpPacket.Checksum, 16);
            stackPanel.Children.Add(new TextBlock { Text = "Checksum: 0x" + checksum });

            if (udpPacket.HasPayloadData)
            {
                string payload = udpPacket.PayloadData.Length.ToString();
                stackPanel.Children.Add(new TextBlock { Text = "Urgent Pointer: " + payload });
            }

            udpExpander.DataContext = udpPacket.Bytes;
            return udpExpander;
        }
    }
}
