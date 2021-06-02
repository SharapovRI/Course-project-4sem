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
    public class ICMPv6ProtocolParser
    {
        public static Expander GetICMPv6ProtocolInfo(DisplayedPacket packet)
        {
            if (packet is null) return null;

            Expander icmpv6Expander = new Expander();
            StackPanel stackPanel = new StackPanel();
            icmpv6Expander.Content = stackPanel;

            IcmpV6Packet icmpPacket = packet.Packet.Extract<IcmpV6Packet>();

            string type = icmpPacket.Type.ToString();
            stackPanel.Children.Add(new TextBlock { Text = "Type: " + type });

            string code = icmpPacket.Code.ToString();
            stackPanel.Children.Add(new TextBlock { Text = "Code: " + code });

            string checkSum = Convert.ToString(icmpPacket.Checksum, 16);
            stackPanel.Children.Add(new TextBlock { Text = "CheckSum: 0x" + checkSum });

            byte[] icmpBytes = icmpPacket.Bytes;
            try
            {
                byte[] reservedBytes = icmpBytes[4..8];
                string reserved = "";
                for (int i = 0; i < reservedBytes.Length; i++)
                {
                    reserved += reservedBytes[i].ToString();
                }
                stackPanel.Children.Add(new TextBlock { Text = "Resesrved: " + reserved });

                byte[] targetAddressBytes = icmpBytes[8..24];
                string targetAddress = "";
                for (int i = 0; i < targetAddressBytes.Length; i++)
                {
                    targetAddress += targetAddressBytes[i].ToString();
                    if (i < 15)
                    {
                        targetAddress += ":";
                    }
                }
                stackPanel.Children.Add(new TextBlock { Text = "Target Address: " + targetAddress });
            }
            catch (IndexOutOfRangeException)
            { }
            return icmpv6Expander;
        }
    }
}
