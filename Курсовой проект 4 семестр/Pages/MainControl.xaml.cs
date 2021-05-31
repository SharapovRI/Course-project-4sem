using SharpPcap;
using SharpPcap.Npcap;
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

namespace Курсовой_проект_4_семестр.Pages
{
    /// <summary>
    /// Логика взаимодействия для MainControl.xaml
    /// </summary>
    public partial class MainControl : UserControl
    {
        private static MainWindow MainWindow;
        public MainControl(MainWindow mainWindow)
        {
            MainWindow = mainWindow;
            InitializeComponent();
            InitDevices();
        }

        public void InitDevices()
        {
            CaptureDeviceList devices = CaptureDeviceList.Instance;
            if (devices.Count < 1)
            {
                Console.WriteLine("No devices were found on this machine");
                return;
            }

            foreach (ICaptureDevice dev in devices)
            {
                NpcapDevice npcapDevice = (NpcapDevice)dev;

                if (npcapDevice.Interface.FriendlyName != null)
                {
                    listOfDevices.Items.Add(new ListViewItem { Content = npcapDevice.Interface.FriendlyName, DataContext = dev });
                }
                else
                {
                    listOfDevices.Items.Add(new ListViewItem { Content = npcapDevice.Interface.Description, DataContext = dev });
                }
            }
        }

        private void listOfDevices_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ListView listView = sender as ListView;
            ListViewItem selectedItem = (ListViewItem)listView.SelectedItem;
            ICaptureDevice device = (ICaptureDevice)selectedItem.DataContext;

            CapturedPacketsControl capturedPacketsControl = new(device, MainWindow);
            MainWindow.MainControler.Content = capturedPacketsControl;
            capturedPacketsControl.DisplayPackets();
        }
    }
}
