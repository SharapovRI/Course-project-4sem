using System.Windows;
using Курсовой_проект_4_семестр.Pages;

namespace Курсовой_проект_4_семестр
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            MainControl mainControl = new MainControl(this);
            MainControler.Content = mainControl;
        }
    }
}
