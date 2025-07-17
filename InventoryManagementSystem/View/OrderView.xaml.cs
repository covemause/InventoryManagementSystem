using System;
using System.Collections.Generic;
using System.Windows;
using InventoryManagementSystem.Controller;
using InventoryManagementSystem.DataBase.Model;
using Microsoft.Win32;
using System.IO;
using System.Globalization;

namespace InventoryManagementSystem.View
{
    public partial class OrderView : Window
    {
        private readonly OrderController _controller = new OrderController();
        private Order _selectedOrder = null;

        public OrderView()
        {
            InitializeComponent();
            LoadOrders();
        }

        private void LoadOrders()
        {
            OrderDataGrid.ItemsSource = _controller.GetAllOrders();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            var order = new Order
            {
                ProductId = int.TryParse(ProductIdTextBox.Text, out int pid) ? pid : 0,
                ProductName = ProductNameTextBox.Text,
                CustomerId = int.TryParse(CustomerIdTextBox.Text, out int cid) ? cid : 0,
                Quantity = int.TryParse(QuantityTextBox.Text, out int qty) ? qty : 0,
                UnitPrice = decimal.TryParse(UnitPriceTextBox.Text, out decimal up) ? up : 0,
                Amount = decimal.TryParse(AmountTextBox.Text, out decimal amt) ? amt : 0,
                OrderDate = OrderDatePicker.SelectedDate ?? DateTime.Now,
                Note = NoteTextBox.Text,
                CreatedAt = DateTime.Now,
                CreatedBy = Environment.UserName,
                UpdatedAt = DateTime.Now,
                UpdatedBy = Environment.UserName
            };
            _controller.AddOrder(order);
            LoadOrders();
            ClearInput();
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedOrder == null) return;
            _selectedOrder.ProductId = int.TryParse(ProductIdTextBox.Text, out int pid) ? pid : 0;
            _selectedOrder.ProductName = ProductNameTextBox.Text;
            _selectedOrder.CustomerId = int.TryParse(CustomerIdTextBox.Text, out int cid) ? cid : 0;
            _selectedOrder.Quantity = int.TryParse(QuantityTextBox.Text, out int qty) ? qty : 0;
            _selectedOrder.UnitPrice = decimal.TryParse(UnitPriceTextBox.Text, out decimal up) ? up : 0;
            _selectedOrder.Amount = decimal.TryParse(AmountTextBox.Text, out decimal amt) ? amt : 0;
            _selectedOrder.OrderDate = OrderDatePicker.SelectedDate ?? DateTime.Now;
            _selectedOrder.Note = NoteTextBox.Text;
            _selectedOrder.UpdatedAt = DateTime.Now;
            _selectedOrder.UpdatedBy = Environment.UserName;
            _controller.UpdateOrder(_selectedOrder);
            LoadOrders();
            ClearInput();
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedOrder == null) return;
            _controller.DeleteOrder(_selectedOrder.Id);
            LoadOrders();
            ClearInput();
        }

        private void OrderDataGrid_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            _selectedOrder = OrderDataGrid.SelectedItem as Order;
            if (_selectedOrder != null)
            {
                ProductIdTextBox.Text = _selectedOrder.ProductId.ToString();
                ProductNameTextBox.Text = _selectedOrder.ProductName ?? "";
                CustomerIdTextBox.Text = _selectedOrder.CustomerId.ToString();
                QuantityTextBox.Text = _selectedOrder.Quantity.ToString();
                UnitPriceTextBox.Text = _selectedOrder.UnitPrice.ToString();
                AmountTextBox.Text = _selectedOrder.Amount.ToString();
                OrderDatePicker.SelectedDate = _selectedOrder.OrderDate;
                NoteTextBox.Text = _selectedOrder.Note ?? "";
            }
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            int? productId = int.TryParse(SearchProductIdTextBox.Text, out int pid) ? pid : (int?)null;
            string productName = SearchProductNameTextBox.Text;
            int? customerId = int.TryParse(SearchCustomerIdTextBox.Text, out int cid) ? cid : (int?)null;
            int? quantity = int.TryParse(SearchQuantityTextBox.Text, out int qty) ? qty : (int?)null;
            decimal? unitPrice = decimal.TryParse(SearchUnitPriceTextBox.Text, out decimal up) ? up : (decimal?)null;
            decimal? amount = decimal.TryParse(SearchAmountTextBox.Text, out decimal amt) ? amt : (decimal?)null;
            DateTime? dateFrom = SearchOrderDateFromPicker.SelectedDate;
            DateTime? dateTo = SearchOrderDateToPicker.SelectedDate;
            string note = SearchNoteTextBox.Text;
            bool? isOnlineOrder = OnlineOrderOnlyCheckBox.IsChecked == true ? true : (bool?)null;

            var all = _controller.GetAllOrders();
            var filtered = all;
            if (dateFrom.HasValue || dateTo.HasValue)
            {
                filtered = filtered.FindAll(o =>
                    (!dateFrom.HasValue || o.OrderDate >= dateFrom.Value) &&
                    (!dateTo.HasValue || o.OrderDate <= dateTo.Value)
                );
            }
            if (isOnlineOrder == true)
            {
                filtered = filtered.FindAll(o => o.IsOnlineOrder);
            }
            if (productId.HasValue)
                filtered = filtered.FindAll(o => o.ProductId == productId.Value);
            if (!string.IsNullOrEmpty(productName))
                filtered = filtered.FindAll(o => o.ProductName.Contains(productName));
            if (customerId.HasValue)
                filtered = filtered.FindAll(o => o.CustomerId == customerId.Value);
            if (quantity.HasValue)
                filtered = filtered.FindAll(o => o.Quantity == quantity.Value);
            if (unitPrice.HasValue)
                filtered = filtered.FindAll(o => o.UnitPrice == unitPrice.Value);
            if (amount.HasValue)
                filtered = filtered.FindAll(o => o.Amount == amount.Value);
            if (!string.IsNullOrEmpty(note))
                filtered = filtered.FindAll(o => o.Note != null && o.Note.Contains(note));
            OrderDataGrid.ItemsSource = filtered;
        }

        private void ClearSearchButton_Click(object sender, RoutedEventArgs e)
        {
            SearchProductIdTextBox.Text = "";
            SearchProductNameTextBox.Text = "";
            SearchCustomerIdTextBox.Text = "";
            SearchQuantityTextBox.Text = "";
            SearchUnitPriceTextBox.Text = "";
            SearchAmountTextBox.Text = "";
            SearchOrderDateFromPicker.SelectedDate = null;
            SearchOrderDateToPicker.SelectedDate = null;
            SearchNoteTextBox.Text = "";
            OnlineOrderOnlyCheckBox.IsChecked = false;
            LoadOrders();
        }

        private void ImportCsvButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog
            {
                Filter = "CSVファイル (*.csv)|*.csv"
            };
            if (dialog.ShowDialog() == true)
            {
                try
                {
                    var lines = File.ReadAllLines(dialog.FileName);
                    if (lines.Length < 2) return; // ヘッダー＋1行未満
                    for (int i = 1; i < lines.Length; i++)
                    {
                        var cols = lines[i].Split(',');
                        if (cols.Length < 8) continue;
                        var order = new Order
                        {
                            ProductId = int.TryParse(cols[0], out int pid) ? pid : 0,
                            ProductName = cols[1],
                            CustomerId = int.TryParse(cols[2], out int cid) ? cid : 0,
                            Quantity = int.TryParse(cols[3], out int qty) ? qty : 0,
                            UnitPrice = decimal.TryParse(cols[4], out decimal up) ? up : 0,
                            Amount = decimal.TryParse(cols[5], out decimal amt) ? amt : 0,
                            OrderDate = DateTime.TryParseExact(cols[6], new[] { "yyyy/MM/dd", "yyyy-MM-dd" }, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime od) ? od : DateTime.Now,
                            Note = cols[7],
                            IsOnlineOrder = true,
                            CreatedAt = DateTime.Now,
                            CreatedBy = Environment.UserName,
                            UpdatedAt = DateTime.Now,
                            UpdatedBy = Environment.UserName
                        };
                        _controller.AddOrder(order);
                    }
                    LoadOrders();
                    MessageBox.Show("CSVインポートが完了しました。", "完了", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"インポート中にエラーが発生しました: {ex.Message}", "エラー", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void ClearInput()
        {
            ProductIdTextBox.Text = "";
            ProductNameTextBox.Text = "";
            CustomerIdTextBox.Text = "";
            QuantityTextBox.Text = "";
            UnitPriceTextBox.Text = "";
            AmountTextBox.Text = "";
            OrderDatePicker.SelectedDate = DateTime.Now;
            NoteTextBox.Text = "";
            _selectedOrder = null;
            OrderDataGrid.UnselectAll();
        }
    }
} 