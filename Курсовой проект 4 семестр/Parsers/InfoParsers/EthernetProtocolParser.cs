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
    public class EthernetProtocolParser
    {
        public static Expander GetEthernetInfo(DisplayedPacket packet)
        {
            if (packet is null) return null;

            Expander ethernetExpander = new Expander();
            StackPanel stackPanel = new StackPanel();
            ethernetExpander.Content = stackPanel;

            EthernetPacket ethernet = packet.Packet.Extract<EthernetPacket>();
            if (ethernet is null) return null;

            byte[] destBytes = ethernet.DestinationHardwareAddress.GetAddressBytes();
            string dest = "";
            for (int i = 0; i < destBytes.Length; i++)
            {
                dest += Convert.ToString(destBytes[i], 16);
                if (i < 5) dest += ":";
            }
            stackPanel.Children.Add(new TextBlock { Text = "Destination Addres: " + dest });


            byte[] sourceBytes = ethernet.SourceHardwareAddress.GetAddressBytes();
            string source = "";
            for (int i = 0; i < sourceBytes.Length; i++)
            {
                source += Convert.ToString(sourceBytes[i], 16);
                if (i < 5) source += ":";
            }
            stackPanel.Children.Add(new TextBlock { Text = "Source Addres: " + source });

            string type = ethernet.Type.ToString();
            stackPanel.Children.Add(new TextBlock { Text = "Type: " + type });


            ethernetExpander.DataContext = ethernet.Bytes;
            return ethernetExpander;
        }
    }
}
