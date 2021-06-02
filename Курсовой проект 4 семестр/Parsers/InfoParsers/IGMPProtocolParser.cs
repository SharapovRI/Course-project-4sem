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
    public class IGMPProtocolParser
    {
        public static Expander GetIGMPProtocolInfo(DisplayedPacket packet)
        {
            if (packet is null) return null;

            Expander igmpExpander = new Expander();
            StackPanel stackPanel = new StackPanel();
            igmpExpander.Content = stackPanel;

            IgmpV2Packet igmpPacket = packet.Packet.Extract<IgmpV2Packet>();
            if (igmpPacket is null) return null;

            string type = igmpPacket.Type.ToString();
            stackPanel.Children.Add(new TextBlock { Text = "Type: " + type });

            string checksum = Convert.ToString(igmpPacket.Checksum, 16);
            stackPanel.Children.Add(new TextBlock { Text = "Checksum: 0x" + checksum });

            string group = igmpPacket.GroupAddress.ToString();
            stackPanel.Children.Add(new TextBlock { Text = "Group Address: " + group });

            string len = igmpPacket.MaxResponseTime.ToString();
            stackPanel.Children.Add(new TextBlock { Text = "Max Response Time: " + len });

            igmpExpander.DataContext = igmpPacket.Bytes;
            return igmpExpander;
        }
    }
}

