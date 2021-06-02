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
    public class DNSProtocolParser
    {
        public static Expander GetDNSInfo(DisplayedPacket packet)
        {
            if (packet is null) return null;

            Expander DNSExpander = new Expander();
            StackPanel stackPanel = new StackPanel();
            DNSExpander.Content = stackPanel;

            UdpPacket udpPacket = packet.Packet.Extract<UdpPacket>();
            if (udpPacket is null) return null;
            byte[] dnsPacket = udpPacket.PayloadData;
            if (dnsPacket is null) return null;

            try
            {
                byte[] transIDBytes = dnsPacket[0..2];
                string transID = "";
                for (int i = 0; i < transIDBytes.Length; i++)
                {
                    transID += Convert.ToString(transIDBytes[i], 16);
                }
                stackPanel.Children.Add(new TextBlock { Text = "Transaction ID: 0x" + transID });


                byte[] flagsBytes = dnsPacket[2..4];
                string flags = "";
                for (int i = 0; i < flagsBytes.Length; i++)
                {
                    string oneByte = Convert.ToString(flagsBytes[i], 16);
                    if (oneByte.Length < 2)
                    {
                        oneByte = "0" + oneByte;
                    }
                    flags += oneByte;
                }
                stackPanel.Children.Add(new TextBlock { Text = "Flags: 0x" + flags });


                byte[] qestBytes = dnsPacket[4..6];
                string qest = "";
                for (int i = 0; i < qestBytes.Length; i++)
                {
                    qest += qestBytes[i].ToString();
                }

                stackPanel.Children.Add(new TextBlock { Text = "Questions: " + Convert.ToInt16(qest,16) });


                byte[] ansBytes = dnsPacket[6..8];
                string ans = "";
                for (int i = 0; i < ansBytes.Length; i++)
                {
                    ans += ansBytes[i].ToString();
                }

                stackPanel.Children.Add(new TextBlock { Text = "Answer RRs: " + Convert.ToInt16(ans, 16) });


                byte[] authBytes = dnsPacket[8..10];
                string auth = "";
                for (int i = 0; i < authBytes.Length; i++)
                {
                    auth += authBytes[i].ToString();
                }

                stackPanel.Children.Add(new TextBlock { Text = "Authority RRs: " + Convert.ToInt16(auth, 16) });
            }
            catch (IndexOutOfRangeException)
            { }

            return DNSExpander;
        }
    }
}
