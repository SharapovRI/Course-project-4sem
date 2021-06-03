using PacketDotNet;
using SharpPcap;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
                     DisplayedPacket packet;
                     if (device.MacAddress != null)
                     {
                         packet = PacketParser.ParsePacket(e, ID++, device.MacAddress.ToString());
                     }
                     else
                     {
                         packet = PacketParser.ParsePacket(e, ID++);
                     }
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
            Back.IsEnabled = true;
        }

        private void listOfPackets_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DetailedFrame.Children.Clear();
            DisplayedPacket packet = listOfPackets.SelectedItem as DisplayedPacket;

            Expander frameExpander = ProtocolParser.ParseProtocol(packet, "Frame");
            if (frameExpander != null)
            {
                frameExpander.Header = "Frame " + packet.Number;
                frameExpander.FontSize = 14;
                frameExpander.MouseDown += Expander_MouseDown;
                DetailedFrame.Children.Add(frameExpander);
            }


            List<string> namesOfProtocols = PacketParser.GetListOfProtocols(packet);
            if (namesOfProtocols is null) return;
            foreach (var protocol in namesOfProtocols)
            {
                Expander expander = ProtocolParser.ParseProtocol(packet, protocol);
                if (expander != null)
                {
                    expander.Header = protocol + ": ";
                    expander.FontSize = 14;
                    expander.MouseDown += Expander_MouseDown;
                    DetailedFrame.Children.Add(expander);
                }
            }
        }

        private void Expander_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Expander expander = sender as Expander;
            byte[] bytes = (byte[])expander.DataContext;
            if (bytes == null) return;

            string str = "";
            for (int i = 0; i < bytes.Length; i++)
            {
                string tecByte = Convert.ToString(bytes[i], 16);
                if (tecByte.Length < 2) tecByte = "0" + tecByte;
                str += tecByte + " ";
                if ((i + 1) % 8 == 0)
                {
                    str += "\t";
                }
                if ((i + 1) % 32 == 0)
                {
                    str += "\n";
                }
            }

            TBBytes.Text = str;
        }

        private void Resume_Click(object sender, RoutedEventArgs e)
        {
            resumeBut.IsEnabled = false;
            stopBut.IsEnabled = true;
            Back.IsEnabled = false;

            device.OnPacketArrival += new PacketArrivalEventHandler(Device_OnPacketArrival);
            // Open the device for capturing
            int readTimeoutMilliseconds = 1000;
            device.Open(DeviceMode.Promiscuous, readTimeoutMilliseconds);

            // Start the capturing process
            device.StartCapture();
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            MainControl mainControl = new MainControl(MainWindow);
            MainWindow.MainControler.Content = mainControl;
        }



        GridViewColumnHeader _lastHeaderClicked = null;
        bool isAscending = false;
        private void ListViewColumnHeaderClick(object sender, RoutedEventArgs e)
        {
            GridViewColumnHeader headerClicked = e.OriginalSource as GridViewColumnHeader;

            if (headerClicked == null)
                return;

            if (headerClicked.Role == GridViewColumnHeaderRole.Padding)
                return;

            var sortingColumn = (headerClicked.Column.DisplayMemberBinding as Binding)?.Path?.Path;
            if (sortingColumn == null)
                return;

            if (_lastHeaderClicked == headerClicked)
            {
                if (isAscending)
                {
                    listOfPackets.Items.SortDescriptions.Clear();
                    listOfPackets.Items.SortDescriptions.Add(new SortDescription(sortingColumn, ListSortDirection.Descending));
                    isAscending = false;
                }
                else
                {
                    listOfPackets.Items.SortDescriptions.Clear();
                    listOfPackets.Items.SortDescriptions.Add(new SortDescription(sortingColumn, ListSortDirection.Ascending));
                    isAscending = true;
                }
                return;
            }

            _lastHeaderClicked = headerClicked;
            isAscending = true;
            listOfPackets.Items.SortDescriptions.Clear();
            listOfPackets.Items.SortDescriptions.Add(new SortDescription(sortingColumn, ListSortDirection.Ascending));
        }
        
    }
}
