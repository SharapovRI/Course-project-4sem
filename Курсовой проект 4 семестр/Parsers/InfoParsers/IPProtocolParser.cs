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
    public class IPProtocolParser
    {
        public static Expander GetIPPacketInfo(DisplayedPacket packet)
        {
            if (packet is null) return null;

            Expander ipExpander = new Expander();
            StackPanel stackPanel = new StackPanel();
            ipExpander.Content = stackPanel;

            IPPacket pPacket = packet.Packet.Extract<IPPacket>();
            if (pPacket is null) return null;

            string vers = pPacket.Version.ToString();
            stackPanel.Children.Add(new TextBlock { Text = "Version: " + vers });

            string headerLen = pPacket.HeaderLength.ToString();
            stackPanel.Children.Add(new TextBlock { Text = "Header Length: " + headerLen });

            string totalLen = pPacket.TotalLength.ToString();
            stackPanel.Children.Add(new TextBlock { Text = "Total Length: " + totalLen });

            string timeToLive = pPacket.TimeToLive.ToString();
            stackPanel.Children.Add(new TextBlock { Text = "Time To Live: " + timeToLive });

            string source = pPacket.SourceAddress.ToString();
            stackPanel.Children.Add(new TextBlock { Text = "Source Address: " + source });

            string destin = pPacket.DestinationAddress.ToString();
            stackPanel.Children.Add(new TextBlock { Text = "Destination Address: " + destin });

            string defServField = "";
            string identification = "";
            string flags = "";
            string fragmentOffset = "";
            string headerSum = "";
            try
            {

                byte[] data = pPacket.Bytes;
                defServField = "0x" + Convert.ToString(data[1],16);
                identification = "0x" + Convert.ToString(data[4], 16) + Convert.ToString(data[5], 16);
                flags = "0x" + Convert.ToString(data[6], 16);
                fragmentOffset = Convert.ToString(data[7], 16);
                headerSum = "0x" + Convert.ToString(data[10], 16) + Convert.ToString(data[11], 16);

                stackPanel.Children.Add(new TextBlock { Text = "Differentiated Services Field: " + defServField });
                stackPanel.Children.Add(new TextBlock { Text = "Identification: " + identification });
                stackPanel.Children.Add(new TextBlock { Text = "Flags: " + flags });
                stackPanel.Children.Add(new TextBlock { Text = "Fragment Offset: " + fragmentOffset });
                stackPanel.Children.Add(new TextBlock { Text = "Header Checksum: " + headerSum });
            }
            catch (IndexOutOfRangeException)
            { }
            return ipExpander;
        }
    }
}
