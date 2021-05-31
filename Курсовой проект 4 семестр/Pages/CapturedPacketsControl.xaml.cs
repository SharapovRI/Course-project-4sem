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
                     var packet = PacketParser.ParsePacket(e, ID++);
                     if (packet != null)
                     {
                         listOfPackets.Items.Add(packet);
                     }
                 });
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // Stop the capturing process
            device.StopCapture();

            // Close the pcap device
            device.Close();
        }

        private void listOfPackets_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DisplayedPacket a = listOfPackets.SelectedItem as DisplayedPacket;
        }
    }
}
