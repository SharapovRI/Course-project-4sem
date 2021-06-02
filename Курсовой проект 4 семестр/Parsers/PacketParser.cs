using PacketDotNet;
using SharpPcap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using Курсовой_проект_4_семестр.Models;

namespace Курсовой_проект_4_семестр.Parsers
{
    public class PacketParser
    {
        private static string MacAddress = "";
        public static List<string> GetListOfProtocols(DisplayedPacket displayedPacket)
        {
            List<string> listOFPackets = new List<string>();

            Packet packet = displayedPacket.Packet;
            if (packet is null) return null;

            if (packet.HasPayloadPacket)
            {
                listOFPackets.Add(packet.GetType().Name);
                do
                {
                    packet = packet.PayloadPacket;
                    listOFPackets.Add(packet.GetType().Name);
                }
                while (packet.HasPayloadPacket);
            }

            switch (displayedPacket.Protocol)
            {
                case "TLS":
                    {
                        listOFPackets.Add("Transport Layer Security");
                        break;
                    }
                case "BROWSER":
                    {
                        listOFPackets.Add("NetBIOS Datagram Service");
                        listOFPackets.Add("SMB (Server Message Block Protocol");
                        listOFPackets.Add("SMB MailSlot Protocol");
                        listOFPackets.Add("Microsoft Windows Browser Protocol");
                        break;
                    }
                case "DNS":
                    {
                        listOFPackets.Add("Domain Name System");
                        break;
                    }
                case "MDNS":
                    {
                        listOFPackets.Add("Multicast Domain Name System");
                        break;
                    }
                case "LLMNR":
                    {
                        listOFPackets.Add("Link-local Multicast Name Resolution");
                        break;
                    }
                case "NBNS":
                    {
                        listOFPackets.Add("NetBIOS Name Service");
                        break;
                    }
                case "SSDP":
                    {
                        listOFPackets.Add("Simple Service Discovery Protocol");
                        break;
                    }
                case "HTTP":
                    {
                        listOFPackets.Add("Hypertext Transfer Protocol");
                        break;
                    }
                default:
                    break;
            }

            return listOFPackets;
        }
        public static DisplayedPacket ParsePacket(CaptureEventArgs capturedData, int id, string macAddress = "")
        {
            MacAddress = macAddress;
            Packet allPackets = capturedData.Packet.GetPacket();
            EthernetPacket ethernet = allPackets.Extract<EthernetPacket>();
            if (allPackets is null) return null;

            Packet lastPacket = GetLastPacket(allPackets);

            string sourceAddress = "";
            string destinationAddress = "";
            if (!GetAddresses(allPackets, ref sourceAddress, ref destinationAddress)) return null;
            string protocol = GetProtocol(allPackets);
            return new DisplayedPacket(id, capturedData.Packet.Timeval, sourceAddress,
                destinationAddress, protocol, capturedData.Packet.Data.Length, allPackets, lastPacket, capturedData);
        }

        private static Packet GetLastPacket(Packet captureEvent)
        {
            Packet packet = captureEvent;

            if (packet is null) return null;

            while (packet.HasPayloadPacket)
            {
                packet = packet.PayloadPacket;
            }

            return packet;
        }

        private static bool GetAddresses(Packet packet, ref string source, ref string destination)
        {
            IPPacket iPPacket = packet.Extract<IPPacket>();

            if (iPPacket is null)
            {
                EthernetPacket ethernet = packet.Extract<EthernetPacket>();
                if (ethernet == null) return false;

                source = ethernet.SourceHardwareAddress.ToString();
                destination = ethernet.DestinationHardwareAddress.ToString();

                return true;
            }

            source = iPPacket.SourceAddress.ToString();
            destination = iPPacket.DestinationAddress.ToString();
            return true;
        }

        private static string GetProtocol(Packet allPackets)
        {
            Packet packet = GetLastPacket(allPackets);
            if (packet.GetType().Name == "TcpPacket")
            {
                TcpPacket tcpPacket = (TcpPacket)packet;
                if (CheckHTTP(tcpPacket)) return "HTTP";
                int preflags = int.Parse(Convert.ToString(tcpPacket.Flags, 2));
                int flags = preflags;
                if (flags / 1000 % 100 == 11 && tcpPacket.HasPayloadData)
                {
                    return "TLS";
                }
            }

            if (packet.GetType().Name == "UdpPacket")
            {
                EthernetPacket ethernet = allPackets.Extract<EthernetPacket>();
                if (ethernet != null)
                {
                    if (CheckMDNS(ethernet)) return "MDNS";
                    if (CheckDNS(ethernet)) return "DNS";
                    if (CheckNBNS(ethernet)) return "NBNS";
                    if (CheckLLMNR(ethernet)) return "LLMNR";
                    if (CheckSSDP(ethernet)) return "SSDP";
                    if (CheckBROWSER(ethernet)) return "BROWSER";
                    if (CheckHTTP(ethernet)) return "HTTP";
                }
            }

            return packet.GetType().Name;
        }

        private static bool CheckMDNS(EthernetPacket ethernet)
        {
            UdpPacket udpPacket = ethernet.Extract<UdpPacket>();

            if (udpPacket.DestinationPort == 5353 || udpPacket.SourcePort == 5353) return true;

            return false;
        }

        private static bool CheckDNS(EthernetPacket ethernet)
        {
            UdpPacket udpPacket = ethernet.Extract<UdpPacket>();

            if (udpPacket.DestinationPort == 53 || udpPacket.SourcePort == 53) return true;

            return false;
        }

        private static bool CheckSSDP(EthernetPacket ethernet)
        {
            PhysicalAddress source = ethernet.SourceHardwareAddress;
            PhysicalAddress dest = ethernet.DestinationHardwareAddress;

            if (source.ToString() != MacAddress)
            {
                return false;
            }


            if (dest.ToString() != "01005E7FFFFA")
            {
                return false;
            }

            return true;
        }

        private static bool CheckNBNS(EthernetPacket ethernet)
        {
            PhysicalAddress source = ethernet.SourceHardwareAddress;
            PhysicalAddress dest = ethernet.DestinationHardwareAddress;

            if (source.ToString() != MacAddress)
            {
                return false;
            }


            if (dest.ToString() != "FFFFFFFFFFFF")
            {
                return false;
            }

            UdpPacket udpPacket = (UdpPacket)GetLastPacket(ethernet);

            if (udpPacket.DestinationPort != 137)
            {
                return false;
            }

            return true; // 01005E7FFFFA
        }

        private static bool CheckBROWSER(EthernetPacket ethernet)
        {
            /*PhysicalAddress source = ethernet.SourceHardwareAddress;
            PhysicalAddress dest = ethernet.DestinationHardwareAddress;

            if (source.ToString() != MacAddress)
            {
                return false;
            }


            if (dest.ToString() != "FFFFFFFFFFFF")
            {
                return false;
            }*/

            UdpPacket udpPacket = (UdpPacket)GetLastPacket(ethernet);

            if (udpPacket.DestinationPort != 138 || udpPacket.SourcePort != 138)
            {
                return false;
            }

            return true;
        }

        private static bool CheckLLMNR(EthernetPacket ethernet)
        {
            PhysicalAddress source = ethernet.SourceHardwareAddress;
            PhysicalAddress dest = ethernet.DestinationHardwareAddress;
            
            if (source.ToString() != MacAddress)
            {
                return false;
            }


            if (dest.ToString() != "01005E0000FC" && dest.ToString() != "333300010003")
            {
                return false;
            }

            return true;
        }

        private static bool CheckHTTP(TcpPacket tcpPacket)
        {
            if (tcpPacket.DestinationPort == 80 || tcpPacket.SourcePort == 80) return true;

            return false;
        }

        private static bool CheckHTTP(EthernetPacket ethernet)
        {
            UdpPacket udpPacket = ethernet.Extract<UdpPacket>();
            if (udpPacket.DestinationPort == 80 || udpPacket.SourcePort == 80) return true;

            return false;
        }        
    }
}
