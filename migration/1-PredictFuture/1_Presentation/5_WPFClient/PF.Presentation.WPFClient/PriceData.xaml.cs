using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using PF.Application.Dto.StockData;
using PF.Application.StockData.ServiceImpl;

namespace PF.Presentation.WPFClient
{
    /// <summary>
    /// PriceData.xaml 的交互逻辑
    /// </summary>
    public partial class PriceData : UserControl
    {
        public PriceData()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var symbol = _textBoxSymbol.Text;
            var startday = _datePickerStart.SelectedDate;
            var endday = _datePickerEnd.SelectedDate;
            if (string.IsNullOrEmpty(symbol) || startday == null || endday == null)
            {
                return;
            }

            var items = new PriceDataAppService().GetDaylineData(symbol, startday.Value, endday.Value);
            _dataGridPriceDate.ItemsSource = new ObservableCollection<PriceDataItemDto>(items);
        }
    }
}
