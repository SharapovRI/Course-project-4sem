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
    public class SSDPParser
    {
        public static Expander GetSSDPInfo(DisplayedPacket packet)
        {
            if (packet is null) return null;

            Expander SSDPExpander = new Expander();
            StackPanel stackPanel = new StackPanel();
            SSDPExpander.Content = stackPanel;

            UdpPacket udpPacket = packet.Packet.Extract<UdpPacket>();
            if (udpPacket is null) return null;
            byte[] ssdpPacket = udpPacket.PayloadData;
            if (ssdpPacket is null) return null;

            try
            {
                byte[] requestMethodBytes = ssdpPacket[0..8];
                string requestMethod = "";
                requestMethod = Encoding.ASCII.GetString(requestMethodBytes);
                stackPanel.Children.Add(new TextBlock { Text = "Request Method: " + requestMethod });

                byte[] requestURIBytes = new byte[1];
                requestURIBytes[0] = ssdpPacket[9];
                string requestURI = "";
                requestURI = Encoding.ASCII.GetString(requestURIBytes);
                stackPanel.Children.Add(new TextBlock { Text = "Request URI: " + requestURI });

                byte[] requestVersBytes = ssdpPacket[11..19];
                string requestVers = "";
                requestVers = Encoding.ASCII.GetString(requestVersBytes);
                stackPanel.Children.Add(new TextBlock { Text = "Request Version: " + requestVers });
            }
            catch (IndexOutOfRangeException)
            { }

            SSDPExpander.DataContext = ssdpPacket;
            return SSDPExpander;
        }

    }
}
