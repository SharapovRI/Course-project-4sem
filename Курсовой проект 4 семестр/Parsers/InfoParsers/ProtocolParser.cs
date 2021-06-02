using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using Курсовой_проект_4_семестр.Models;

namespace Курсовой_проект_4_семестр.Parsers.InfoParsers
{
    public class ProtocolParser
    {
        public static Expander ParseProtocol(DisplayedPacket packet, string nameOfProtocol)
        {
            switch (nameOfProtocol)
            {
                case "Frame":
                    {
                        return FrameParser.GetFrameInfo(packet);
                    }
                case "EthernetPacket":
                    {
                        return EthernetProtocolParser.GetEthernetInfo(packet);
                    }
                case "IPv4Packet":
                case "IPv6Packet":
                    {
                        return IPProtocolParser.GetIPPacketInfo(packet);
                    }
                case "TcpPacket":
                    {
                        return TCPProtocolParser.GetTCPPacketInfo(packet);
                    }
                case "UdpPacket":
                    {
                        return UDPProtocolParser.GetTCPPacketInfo(packet);
                    }
                case "DhcpPacket":
                    {
                        return DHCPProtocolParser.GetDHCProtocolInfo(packet);
                    }
                case "ArpPacket":
                    {
                        return ARPProtocolParser.GetTCPPacketInfo(packet);
                    }
                case "Simple Service Discovery Protocol":
                    {
                        return SSDPParser.GetSSDPInfo(packet);
                    }
                case "IcmpV4Packet":
                    {
                        return ICMPv4ProtocolParser.GetICMPv4ProtocolInfo(packet);
                    }
                case "IcmpV6Packet":
                    {
                        return ICMPv6ProtocolParser.GetICMPv6ProtocolInfo(packet);
                    }
                case "Domain Name System":
                case "Multicast Domain Name System":
                case "Link-local Multicast Name Resolution":
                    {
                        return DNSProtocolParser.GetDNSInfo(packet);
                    }
                case "Hypertext Transfer Protocol":
                    {
                        return HTTPProtocolParser.GetHTTPInfo(packet);
                    }
                case "IgmpPacket":
                    {
                        return IGMPProtocolParser.GetIGMPProtocolInfo(packet);
                    }
                default:
                    {
                        return null;
                    }
                    break;
            }
        }
    }
}
