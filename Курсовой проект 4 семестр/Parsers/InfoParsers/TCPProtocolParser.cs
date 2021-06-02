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
    public class TCPProtocolParser
    {
        public static Expander GetTCPPacketInfo(DisplayedPacket packet)
        {
            if (packet is null) return null;

            Expander tcpExpander = new Expander();
            StackPanel stackPanel = new StackPanel();
            tcpExpander.Content = stackPanel;

            TcpPacket tcpPacket = packet.Packet.Extract<TcpPacket>();
            if (tcpPacket is null) return null;

            string sourcePort = tcpPacket.SourcePort.ToString();
            stackPanel.Children.Add(new TextBlock { Text = "Source Port: " + sourcePort });

            string destPort = tcpPacket.DestinationPort.ToString();
            stackPanel.Children.Add(new TextBlock { Text = "Destination Port: " + destPort });

            string seqNumber = tcpPacket.SequenceNumber.ToString();
            stackPanel.Children.Add(new TextBlock { Text = "Sequence Number (raw): " + seqNumber });

            string ackNumb = tcpPacket.AcknowledgmentNumber.ToString();
            stackPanel.Children.Add(new TextBlock { Text = "Acknowledgment Number (raw): " + ackNumb });

            string flags = Convert.ToString(tcpPacket.Flags, 16);
            stackPanel.Children.Add(new TextBlock { Text = "Flags: 0x" + flags });

            string window = tcpPacket.WindowSize.ToString();
            stackPanel.Children.Add(new TextBlock { Text = "Window Size: " + window });

            string checksum = Convert.ToString(tcpPacket.Checksum, 16);
            stackPanel.Children.Add(new TextBlock { Text = "Checksum: 0x" + checksum });

            string urgentPointer = tcpPacket.UrgentPointer.ToString();
            stackPanel.Children.Add(new TextBlock { Text = "Urgent Pointer: " + urgentPointer });

            tcpExpander.DataContext = tcpPacket.Bytes;
            return tcpExpander;
        }
    }
}
