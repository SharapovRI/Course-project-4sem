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
        public static DisplayedPacket ParsePacket(CaptureEventArgs capturedData, int id)
        {
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
                    if (CheckNBNS(ethernet)) return "NBNS";
                    if (CheckLLMNR(ethernet)) return "LLMNR";
                    if (CheckSSDP(ethernet)) return "SSDP";
                    if (CheckBROWSER(ethernet)) return "BROWSER";
                }
            }

            return packet.GetType().Name;
        }

        private static bool CheckMDNS(EthernetPacket ethernet)
        {
            PhysicalAddress source = ethernet.SourceHardwareAddress;
            PhysicalAddress dest = ethernet.DestinationHardwareAddress;

            /*byte[] sourceAddress = source.GetAddressBytes();
            if (sourceAddress[0] != 116 || sourceAddress[1] != 229 || sourceAddress[2] != 11 || sourceAddress[3] != 227 || sourceAddress[4] != 5 || sourceAddress[5] != 38)
            {
                return false;
            }*/

            if (source.ToString() != "74E50BE30526")
            {
                return false;
            }


            byte[] destAddress = dest.GetAddressBytes();
            if (destAddress[3] != 00 || destAddress[4] != 00 || destAddress[5] != 251)
            {
                return false;
            }

            return true;
        }

        private static bool CheckSSDP(EthernetPacket ethernet)
        {
            PhysicalAddress source = ethernet.SourceHardwareAddress;
            PhysicalAddress dest = ethernet.DestinationHardwareAddress;

            /*byte[] sourceAddress = source.GetAddressBytes();
            if (sourceAddress[0] != 116 || sourceAddress[1] != 229 || sourceAddress[2] != 11 || sourceAddress[3] != 227 || sourceAddress[4] != 5 || sourceAddress[5] != 38)
            {
                return false;
            }*/

            if (source.ToString() != "74E50BE30526")
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

            /*byte[] sourceAddress = source.GetAddressBytes();
            if (sourceAddress[0] != 116 || sourceAddress[1] != 229 || sourceAddress[2] != 11 || sourceAddress[3] != 227 || sourceAddress[4] != 5 || sourceAddress[5] != 38)
            {
                return false;
            }*/

            if (source.ToString() != "74E50BE30526")
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
            PhysicalAddress source = ethernet.SourceHardwareAddress;
            PhysicalAddress dest = ethernet.DestinationHardwareAddress;

            /*byte[] sourceAddress = source.GetAddressBytes();
            if (sourceAddress[0] != 116 || sourceAddress[1] != 229 || sourceAddress[2] != 11 || sourceAddress[3] != 227 || sourceAddress[4] != 5 || sourceAddress[5] != 38)
            {
                return false;
            }*/

            if (source.ToString() != "74E50BE30526")
            {
                return false;
            }


            if (dest.ToString() != "FFFFFFFFFFFF")
            {
                return false;
            }

            UdpPacket udpPacket = (UdpPacket)GetLastPacket(ethernet);

            if (udpPacket.DestinationPort != 138)
            {
                return false;
            }

            return true;
        }

        private static bool CheckLLMNR(EthernetPacket ethernet)
        {
            PhysicalAddress source = ethernet.SourceHardwareAddress;
            PhysicalAddress dest = ethernet.DestinationHardwareAddress;

            /*byte[] sourceAddress = source.GetAddressBytes();
            if (sourceAddress[0] != 116 || sourceAddress[1] != 229 || sourceAddress[2] != 11 || sourceAddress[3] != 227 || sourceAddress[4] != 5 || sourceAddress[5] != 38)
            {
                return false;
            }*/

            if (source.ToString() != "74E50BE30526")
            {
                return false;
            }


            if (dest.ToString() != "01005E0000FC" && dest.ToString() != "333300010003")
            {
                return false;
            }

            return true;
        }
    }
}
