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
    public class ARPProtocolParser
    {
        public static Expander GetTCPPacketInfo(DisplayedPacket packet)
        {
            if (packet is null) return null;

            Expander ARPExpander = new Expander();
            StackPanel stackPanel = new StackPanel();
            ARPExpander.Content = stackPanel;

            ArpPacket arpPacket = packet.Packet.Extract<ArpPacket>();
            if (arpPacket is null) return null;

            string hardwareType = arpPacket.HardwareAddressType.ToString();
            stackPanel.Children.Add(new TextBlock { Text = "Hardware Address Type: " + hardwareType });

            string protocolType = arpPacket.ProtocolAddressType.ToString();
            stackPanel.Children.Add(new TextBlock { Text = "Protocol Address Type: " + protocolType });

            string hardwareSize = arpPacket.HardwareAddressLength.ToString();
            stackPanel.Children.Add(new TextBlock { Text = "Hardware Address Length: " + hardwareSize });

            string protocolSize = arpPacket.ProtocolAddressLength.ToString();
            stackPanel.Children.Add(new TextBlock { Text = "Protocol Address Length: " + protocolSize });

            string opcode = arpPacket.Operation.ToString();
            stackPanel.Children.Add(new TextBlock { Text = "Operation: " + opcode });

            string senderMacAddress = arpPacket.SenderHardwareAddress.ToString();
            senderMacAddress = senderMacAddress.Insert(10, ":");
            senderMacAddress = senderMacAddress.Insert(8, ":");
            senderMacAddress = senderMacAddress.Insert(6, ":");
            senderMacAddress = senderMacAddress.Insert(4, ":");
            senderMacAddress = senderMacAddress.Insert(2, ":");
            stackPanel.Children.Add(new TextBlock { Text = "Sender Hardware Address: " + senderMacAddress.ToLower() });

            string senderIPAddress = arpPacket.SenderProtocolAddress.ToString();
            stackPanel.Children.Add(new TextBlock { Text = "Sender Protocol Address: " + senderIPAddress });

            string targetMacAddress = arpPacket.TargetHardwareAddress.ToString();
            targetMacAddress = targetMacAddress.Insert(10, ":");
            targetMacAddress = targetMacAddress.Insert(8, ":");
            targetMacAddress = targetMacAddress.Insert(6, ":");
            targetMacAddress = targetMacAddress.Insert(4, ":");
            targetMacAddress = targetMacAddress.Insert(2, ":");
            stackPanel.Children.Add(new TextBlock { Text = "Target Hardware Address: " + targetMacAddress.ToLower() });

            string targetIPAddress = arpPacket.TargetProtocolAddress.ToString();
            stackPanel.Children.Add(new TextBlock { Text = "Target Protocol Address: " + targetIPAddress });

            ARPExpander.DataContext = arpPacket.Bytes;
            return ARPExpander;
        }
    }
}
