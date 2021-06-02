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
    public class ICMPv4ProtocolParser
    {
        public static Expander GetICMPv4ProtocolInfo(DisplayedPacket packet)
        {
            if (packet is null) return null;

            Expander icmpv4Expander = new Expander();
            StackPanel stackPanel = new StackPanel();
            icmpv4Expander.Content = stackPanel;

            IcmpV4Packet icmpPacket = packet.Packet.Extract<IcmpV4Packet>();

            string type = icmpPacket.TypeCode.ToString();
            stackPanel.Children.Add(new TextBlock { Text = "Type: " + type });

            string code = icmpPacket.Id.ToString();
            stackPanel.Children.Add(new TextBlock { Text = "Id: " + code });

            string checkSum = Convert.ToString(icmpPacket.Checksum, 16);
            stackPanel.Children.Add(new TextBlock { Text = "CheckSum: 0x" + checkSum });

            return icmpv4Expander;
        }
    }
}
