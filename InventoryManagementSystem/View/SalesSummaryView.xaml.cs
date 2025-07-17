using System.Windows.Controls;
using InventoryManagementSystem.DataBase.Model;
using InventoryManagementSystem.Controller;
using System.Linq;
using System.Windows;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System;

namespace InventoryManagementSystem.View
{
    /// <summary>
    /// SalesSummaryView.xaml の相互作用ロジック
    /// </summary>
    public partial class SalesSummaryView
    {
        private readonly OrderController _orderController = new OrderController();

        public SalesSummaryView()
        {
            InitializeComponent();
        }

        private void SummaryTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DatePanel == null || MonthPanel == null || CustomerPanel == null || ProductPanel == null) return;
            DatePanel.Visibility = Visibility.Collapsed;
            MonthPanel.Visibility = Visibility.Collapsed;
            CustomerPanel.Visibility = Visibility.Collapsed;
            ProductPanel.Visibility = Visibility.Collapsed;
            switch (SummaryTypeComboBox.SelectedIndex)
            {
                case 0: // 日別
                    DatePanel.Visibility = Visibility.Visible;
                    break;
                case 1: // 月別
                    MonthPanel.Visibility = Visibility.Visible;
                    break;
                case 2: // 顧客別
                    CustomerPanel.Visibility = Visibility.Visible;
                    break;
                case 3: // 商品別
                    ProductPanel.Visibility = Visibility.Visible;
                    break;
            }
        }

        private void SummaryButton_Click(object sender, RoutedEventArgs e)
        {
            var orders = _orderController.GetAllOrders();
            switch (SummaryTypeComboBox.SelectedIndex)
            {
                case 0: // 日別
                    if (TargetDatePicker.SelectedDate == null)
                    {
                        MessageBox.Show("指定日を選択してください。", "エラー", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }
                    var date = TargetDatePicker.SelectedDate.Value.Date;
                    var dayResult = orders.Where(o => o.OrderDate.Date == date)
                        .GroupBy(o => o.OrderDate.Date)
                        .Select(g => new
                        {
                            日付 = g.Key.ToString("yyyy/MM/dd"),
                            件数 = g.Count(),
                            合計数量 = g.Sum(x => x.Quantity),
                            合計金額 = g.Sum(x => x.Amount)
                        }).ToList();
                    SummaryDataGrid.ItemsSource = dayResult;
                    break;
                case 1: // 月別
                    var monthText = TargetMonthTextBox.Text.Trim();
                    if (!System.Text.RegularExpressions.Regex.IsMatch(monthText, @"^\d{4}-\d{2}$"))
                    {
                        MessageBox.Show("指定月はyyyy-MM形式で入力してください。", "エラー", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }
                    var month = monthText;
                    var monthResult = orders.Where(o => o.OrderDate.ToString("yyyy-MM") == month)
                        .GroupBy(o => o.OrderDate.ToString("yyyy-MM"))
                        .Select(g => new
                        {
                            月 = g.Key,
                            件数 = g.Count(),
                            合計数量 = g.Sum(x => x.Quantity),
                            合計金額 = g.Sum(x => x.Amount)
                        }).ToList();
                    SummaryDataGrid.ItemsSource = monthResult;
                    break;
                case 2: // 顧客別
                    if (!int.TryParse(TargetCustomerIdTextBox.Text, out int customerId))
                    {
                        MessageBox.Show("顧客IDを正しく入力してください。", "エラー", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }
                    var customerResult = orders.Where(o => o.CustomerId == customerId)
                        .GroupBy(o => o.CustomerId)
                        .Select(g => new
                        {
                            顧客ID = g.Key,
                            件数 = g.Count(),
                            合計数量 = g.Sum(x => x.Quantity),
                            合計金額 = g.Sum(x => x.Amount)
                        }).ToList();
                    SummaryDataGrid.ItemsSource = customerResult;
                    break;
                case 3: // 商品別
                    if (!int.TryParse(TargetProductIdTextBox.Text, out int productId))
                    {
                        MessageBox.Show("商品IDを正しく入力してください。", "エラー", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }
                    var productResult = orders.Where(o => o.ProductId == productId)
                        .GroupBy(o => o.ProductId)
                        .Select(g => new
                        {
                            商品ID = g.Key,
                            件数 = g.Count(),
                            合計数量 = g.Sum(x => x.Quantity),
                            合計金額 = g.Sum(x => x.Amount)
                        }).ToList();
                    SummaryDataGrid.ItemsSource = productResult;
                    break;
            }
        }
    }
} 