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
    public class DHCPProtocolParser
    {
        public static Expander GetDHCProtocolInfo(DisplayedPacket packet)
        {
            if (packet is null) return null;

            Expander dhcpExpander = new Expander();
            StackPanel stackPanel = new StackPanel();
            dhcpExpander.Content = stackPanel;

            DhcpV4Packet dhcpPacket = packet.Packet.Extract<DhcpV4Packet>();
            if (dhcpPacket is null) return null;

            string mesType = dhcpPacket.MessageType.ToString();
            stackPanel.Children.Add(new TextBlock { Text = "Type: " + mesType });

            string oper = dhcpPacket.Operation.ToString();
            stackPanel.Children.Add(new TextBlock { Text = "Operation: " + oper });

            string hardType = dhcpPacket.HardwareType.ToString();
            stackPanel.Children.Add(new TextBlock { Text = "Hardware Type: " + hardType });

            string hardAddressLen = dhcpPacket.HardwareLength.ToString();
            stackPanel.Children.Add(new TextBlock { Text = "Hardware Length: " + hardAddressLen });

            string hops = dhcpPacket.Hops.ToString();
            stackPanel.Children.Add(new TextBlock { Text = "Hops: " + hops });

            string transID = Convert.ToString(dhcpPacket.TransactionId, 16);
            stackPanel.Children.Add(new TextBlock { Text = "Checksum: 0x" + transID });

            string sec = dhcpPacket.Secs.ToString();
            stackPanel.Children.Add(new TextBlock { Text = "Seconds Elapsed: " + sec });

            string flags = Convert.ToString(dhcpPacket.Flags, 16);
            stackPanel.Children.Add(new TextBlock { Text = "Flags: 0x" + flags });

            string yourAddress = dhcpPacket.YourAddress.ToString();
            stackPanel.Children.Add(new TextBlock { Text = "Your Address: " + yourAddress });

            string clientIP = dhcpPacket.ClientAddress.ToString();
            stackPanel.Children.Add(new TextBlock { Text = "Client IP Address: " + clientIP });

            string gatAddress = dhcpPacket.GatewayAddress.ToString();
            stackPanel.Children.Add(new TextBlock { Text = "Gateaway Address: " + gatAddress });

            string servAddress = dhcpPacket.ServerAddress.ToString();
            stackPanel.Children.Add(new TextBlock { Text = "Server Address: " + servAddress });

            string xid = dhcpPacket.Xid.ToString();
            stackPanel.Children.Add(new TextBlock { Text = "Xid: " + xid });
                       

            return dhcpExpander;
        }
    }
}
