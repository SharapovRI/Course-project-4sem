using PacketDotNet;
using SharpPcap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Курсовой_проект_4_семестр.Models;
using Курсовой_проект_4_семестр.Parsers;
using Курсовой_проект_4_семестр.Parsers.InfoParsers;

namespace Курсовой_проект_4_семестр.Pages
{
    /// <summary>
    /// Логика взаимодействия для CapturedPacketsControl.xaml
    /// </summary>
    public partial class CapturedPacketsControl : UserControl
    {
        private static MainWindow MainWindow;
        private ICaptureDevice device;
        private static int ID = 0;
        public CapturedPacketsControl(ICaptureDevice device, MainWindow mainWindow)
        {
            this.device = device;
            MainWindow = mainWindow;
            InitializeComponent();
           //DisplayPackets();
        }

        public void DisplayPackets()
        {
            ID = 0;
            device.OnPacketArrival += new PacketArrivalEventHandler(Device_OnPacketArrival);

            // Open the device for capturing
            int readTimeoutMilliseconds = 1000;
            device.Open(DeviceMode.Promiscuous, readTimeoutMilliseconds);

            // Start the capturing process
            device.StartCapture();
        }

        private void Device_OnPacketArrival(object sender, CaptureEventArgs e)
        {
            MainWindow.Dispatcher.Invoke(() =>
                 {
                     var packet = PacketParser.ParsePacket(e, ID++, device.MacAddress.ToString());
                     if (packet != null)
                     {
                         listOfPackets.Items.Add(packet);
                     }
                 });
        }

        private void Stop_Click(object sender, RoutedEventArgs e)
        {
            // Stop the capturing process
            device.StopCapture();

            // Close the pcap device
            device.Close();
            
            resumeBut.IsEnabled = true;
            stopBut.IsEnabled = false;
        }

        private void listOfPackets_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DetailedFrame.Children.Clear();
            DisplayedPacket packet = listOfPackets.SelectedItem as DisplayedPacket;

            Expander frameExpander = ProtocolParser.ParseProtocol(packet, "Frame");
            if (frameExpander != null)
            {
                frameExpander.Header = "Frame " + packet.Number;
                DetailedFrame.Children.Add(frameExpander);
            }


            List<string> namesOfProtocols = PacketParser.GetListOfProtocols(packet);

            foreach (var protocol in namesOfProtocols)
            {
                Expander Expander = ProtocolParser.ParseProtocol(packet, protocol);
                if (Expander != null)
                {
                    Expander.Header = protocol + ": ";
                    DetailedFrame.Children.Add(Expander);
                }
            }
        }

        private void Resume_Click(object sender, RoutedEventArgs e)
        {
            resumeBut.IsEnabled = false;
            stopBut.IsEnabled = true;

            device.OnPacketArrival += new PacketArrivalEventHandler(Device_OnPacketArrival);
            // Open the device for capturing
            int readTimeoutMilliseconds = 1000;
            device.Open(DeviceMode.Promiscuous, readTimeoutMilliseconds);

            // Start the capturing process
            device.StartCapture();
        }
    }
}
