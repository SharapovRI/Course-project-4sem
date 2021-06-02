using SharpPcap;
using SharpPcap.Npcap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using Курсовой_проект_4_семестр.Models;

namespace Курсовой_проект_4_семестр.Parsers.InfoParsers
{
    public class FrameParser
    {
        public static Expander GetFrameInfo(DisplayedPacket packet)
        {
            if (packet is null) return null;

            Expander frameExpander = new Expander();
            StackPanel stackPanel = new StackPanel();
            frameExpander.Content = stackPanel;

            string interfaceName = packet.CaptureEvent.Device.Name;
            stackPanel.Children.Add(new TextBlock { Text = "Interface name: " + interfaceName });


            string interfaceDescription = ((NpcapDevice)(packet.CaptureEvent.Device)).Interface.FriendlyName;
            if (interfaceDescription == "")
            {
                interfaceDescription = packet.CaptureEvent.Device.Description;
            }
            stackPanel.Children.Add(new TextBlock { Text = "Interface description: " + interfaceDescription });

            string arrivalTime = packet.CaptureEvent.Packet.Timeval.Date.ToString();
            stackPanel.Children.Add(new TextBlock { Text = "Arrival Time: " + arrivalTime });

            string epochTime = packet.CaptureEvent.Packet.Timeval.ToString();
            stackPanel.Children.Add(new TextBlock { Text = "Epoch Time: " + epochTime });

            string frameNumber = packet.Number.ToString();
            stackPanel.Children.Add(new TextBlock { Text = "Frame Number: " + frameNumber });

            string frameLenth = packet.CaptureEvent.Packet.Data.Length.ToString();
            stackPanel.Children.Add(new TextBlock { Text = "Frame Length: " + frameLenth });

            string capturedLength = packet.Length.ToString();
            stackPanel.Children.Add(new TextBlock { Text = "Captured Length: " + capturedLength });

            return frameExpander;
        }
    }
}
