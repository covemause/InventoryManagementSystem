using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using InventoryManagementSystem.View;

namespace InventoryManagementSystem;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private void OpenCustomerView_Click(object sender, RoutedEventArgs e)
    {
        var window = new CustomerView();
        window.ShowDialog();
    }

    private void OpenInvoiceView_Click(object sender, RoutedEventArgs e)
    {
        var window = new InvoiceView();
        window.ShowDialog();
    }

    private void OpenOrderView_Click(object sender, RoutedEventArgs e)
    {
        var window = new OrderView();
        window.ShowDialog();
    }

    private void OpenPaymentView_Click(object sender, RoutedEventArgs e)
    {
        var window = new PaymentView();
        window.ShowDialog();
    }

    private void OpenSalesSummaryView_Click(object sender, RoutedEventArgs e)
    {
        var window = new SalesSummaryView();
        window.ShowDialog();
    }
}