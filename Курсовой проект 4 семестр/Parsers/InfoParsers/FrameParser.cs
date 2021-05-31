using SharpPcap;
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
        public static Expander GetFrameInfo(CaptureEventArgs packet)
        {
            if (packet is null) return null;

            Expander frameExpander = new Expander();
            StackPanel stackPanel = new StackPanel();
            frameExpander.Content = stackPanel;

            string interfaceName = packet.Device.Name;
            stackPanel.Children.Add(new TextBlock { Text = "Interface name: " + interfaceName });

            string interfaceDescription = packet.Device.Description;
            stackPanel.Children.Add(new TextBlock { Text = "Interface description: " + interfaceDescription });

            string arrivalTime = packet.Packet.Timeval.Date.ToLongTimeString();
            stackPanel.Children.Add(new TextBlock { Text = "Arrival Time: " + arrivalTime });

            string epochTime = packet.Packet.Timeval.ToString();
            stackPanel.Children.Add(new TextBlock { Text = "Epoch Time: " + epochTime });
        }
    }
}
