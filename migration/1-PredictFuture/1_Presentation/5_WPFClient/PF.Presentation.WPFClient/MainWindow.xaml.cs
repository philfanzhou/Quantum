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
using PF.DistributedService.Hosting;
using Xceed.Wpf.AvalonDock.Controls;
using Xceed.Wpf.AvalonDock.Layout;

namespace PF.Presentation.WPFClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            ServiceInitialize.Init();
            InitializeComponent();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            var group = _dock.Layout.RootPanel.Children.OfType<LayoutDocumentPaneGroup>().First();
            if (group.Children.Count == 0)
            {
                var pane = new LayoutDocumentPane();
                pane.Children.Add(new LayoutDocument { Content = new PriceData(), Title = "Price Data" });
                group.Children.Add(pane);
            }
            else
            {
                group.Children.OfType<LayoutDocumentPane>().First().Children.Add(new LayoutDocument { Content = new PriceData(), Title = "Price Data" });
            }
        }
    }
}
