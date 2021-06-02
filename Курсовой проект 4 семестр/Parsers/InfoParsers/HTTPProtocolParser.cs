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
    public class HTTPProtocolParser
    {
        public static Expander GetHTTPInfo(DisplayedPacket packet)
        {
            if (packet is null) return null;

            Expander HTTPExpander = new Expander();
            StackPanel stackPanel = new StackPanel();
            HTTPExpander.Content = stackPanel;

            byte[] httpPacket = null;
            UdpPacket udpPacket;
            TcpPacket tcpPacket = packet.Packet.Extract<TcpPacket>();
            if (tcpPacket is null)
            {
                udpPacket = packet.Packet.Extract<UdpPacket>();
                if (tcpPacket is null) return null;
                httpPacket = udpPacket.PayloadData;
            }
            else
            {
                httpPacket = tcpPacket.PayloadData;
            }
            if (httpPacket is null) return null;

            try
            {
                if (httpPacket[0] == 71)
                {
                    byte[] method = httpPacket.TakeWhile(e => e != 32).ToArray();
                    string requestMethod = "";
                    requestMethod = Encoding.ASCII.GetString(method);
                    stackPanel.Children.Add(new TextBlock { Text = "Request Method: " + requestMethod });


                    httpPacket = httpPacket[(method.Length + 1)..];
                    byte[] requestURIBytes = httpPacket.TakeWhile(e => e != 32).ToArray();
                    string requestURI = "";
                    requestURI = Encoding.ASCII.GetString(requestURIBytes);
                    stackPanel.Children.Add(new TextBlock { Text = "Request URI: " + requestURI });


                    httpPacket = httpPacket[(requestURIBytes.Length + 1)..];
                    byte[] requestVersBytes = httpPacket.TakeWhile(e => e != 13).ToArray();
                    string requestVers = "";
                    requestVers = Encoding.ASCII.GetString(requestVersBytes);
                    stackPanel.Children.Add(new TextBlock { Text = "Request Version: " + requestVers });

                    httpPacket = httpPacket[(requestVersBytes.Length + 2)..];
                    byte[] connectBytes = httpPacket.TakeWhile(e => e != 13).ToArray();
                    string connect = "";
                    connect = Encoding.ASCII.GetString(connectBytes);
                    stackPanel.Children.Add(new TextBlock { Text = "Connection: " + connect });

                    httpPacket = httpPacket[(connectBytes.Length + 2)..];
                    byte[] accertBytes = httpPacket.TakeWhile(e => e != 13).ToArray();
                    string accert = "";
                    accert = Encoding.ASCII.GetString(accertBytes);
                    stackPanel.Children.Add(new TextBlock { Text = "Accept: " + accert });

                    httpPacket = httpPacket[(accertBytes.Length + 2)..];

                    while (httpPacket.Length > 4)
                    {
                        byte[] IMS = httpPacket.TakeWhile(e => e != 13).ToArray();
                        string ims = "";
                        ims = Encoding.ASCII.GetString(IMS);
                        stackPanel.Children.Add(new TextBlock { Text = ims });
                        httpPacket = httpPacket[(IMS.Length + 2)..];
                    }

                }
                else if (httpPacket[0] == 72)
                {
                    byte[] method = httpPacket.TakeWhile(e => e != 32).ToArray();
                    string requestMethod = "";
                    requestMethod = Encoding.ASCII.GetString(method);
                    stackPanel.Children.Add(new TextBlock { Text = "Request Version: " + requestMethod });

                    httpPacket = httpPacket[(method.Length + 1)..];
                    byte[] requestURIBytes = httpPacket.TakeWhile(e => e != 32).ToArray();
                    string requestURI = "";
                    requestURI = Encoding.ASCII.GetString(requestURIBytes);
                    stackPanel.Children.Add(new TextBlock { Text = "Status Code: " + requestURI });

                    httpPacket = httpPacket[(requestURIBytes.Length + 1)..];
                    byte[] requestVersBytes = httpPacket.TakeWhile(e => e != 13).ToArray();
                    string requestVers = "";
                    requestVers = Encoding.ASCII.GetString(requestVersBytes);
                    stackPanel.Children.Add(new TextBlock { Text = "Response Phrase: " + requestVers });

                    httpPacket = httpPacket[(requestVersBytes.Length + 2)..];

                    while (httpPacket.Length > 4)
                    {
                        byte[] IMS = httpPacket.TakeWhile(e => e != 13).ToArray();
                        string ims = "";
                        ims = Encoding.ASCII.GetString(IMS);
                        stackPanel.Children.Add(new TextBlock { Text = ims });
                        httpPacket = httpPacket[(IMS.Length + 2)..];
                    }
                }
            }
            catch (IndexOutOfRangeException)
            { }

            HTTPExpander.DataContext = tcpPacket.PayloadData;
            return HTTPExpander;
        }
    }
}
