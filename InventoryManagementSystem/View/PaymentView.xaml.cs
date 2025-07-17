using InventoryManagementSystem.Controller;
using InventoryManagementSystem.DataBase.Model;

using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace InventoryManagementSystem.View
{
    public partial class PaymentView : Window
    {
        private readonly PaymentController _controller = new PaymentController();
        private Payment _selectedPayment = new();

        public PaymentView()
        {
            InitializeComponent();
            LoadPayments();
        }

        private void LoadPayments()
        {
            PaymentDataGrid.ItemsSource = _controller.GetAllPayments();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            var payment = new Payment
            {
                CustomerId = int.TryParse(CustomerIdTextBox.Text, out int cid) ? cid : 0,
                PaymentDate = PaymentDatePicker.SelectedDate ?? DateTime.Now,
                Amount = decimal.TryParse(AmountTextBox.Text, out decimal amt) ? amt : 0,
                Method = MethodTextBox.Text,
                Note = NoteTextBox.Text,
                CreatedAt = DateTime.Now,
                CreatedBy = Environment.UserName,
                UpdatedAt = DateTime.Now,
                UpdatedBy = Environment.UserName
            };
            _controller.AddPayment(payment);
            LoadPayments();
            ClearInput();
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedPayment == null) return;
            _selectedPayment.CustomerId = int.TryParse(CustomerIdTextBox.Text, out int cid) ? cid : 0;
            _selectedPayment.PaymentDate = PaymentDatePicker.SelectedDate ?? DateTime.Now;
            _selectedPayment.Amount = decimal.TryParse(AmountTextBox.Text, out decimal amt) ? amt : 0;
            _selectedPayment.Method = MethodTextBox.Text;
            _selectedPayment.Note = NoteTextBox.Text;
            _selectedPayment.UpdatedAt = DateTime.Now;
            _selectedPayment.UpdatedBy = Environment.UserName;
            _controller.UpdatePayment(_selectedPayment);
            LoadPayments();
            ClearInput();
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedPayment == null) return;
            _controller.DeletePayment(_selectedPayment.Id);
            LoadPayments();
            ClearInput();
        }

        private void PaymentDataGrid_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (((DataGrid)(sender)).SelectedItem == null) return;
            _selectedPayment = (Payment)((DataGrid)(sender)).SelectedItem;
            if (_selectedPayment != null)
            {
                CustomerIdTextBox.Text = _selectedPayment.CustomerId.ToString();
                PaymentDatePicker.SelectedDate = _selectedPayment.PaymentDate;
                AmountTextBox.Text = _selectedPayment.Amount.ToString();
                MethodTextBox.Text = _selectedPayment.Method ?? "";
                NoteTextBox.Text = _selectedPayment.Note ?? "";
            }
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            int? customerId = int.TryParse(SearchCustomerIdTextBox.Text, out int cid) ? cid : (int?)null;
            DateTime? paymentDate = SearchPaymentDatePicker.SelectedDate;
            decimal? amount = decimal.TryParse(SearchAmountTextBox.Text, out decimal amt) ? amt : (decimal?)null;
            string method = SearchMethodTextBox.Text;
            string note = SearchNoteTextBox.Text;
            PaymentDataGrid.ItemsSource = _controller.SearchPayments(customerId, paymentDate, amount, method, note);
        }

        private void ClearSearchButton_Click(object sender, RoutedEventArgs e)
        {
            SearchCustomerIdTextBox.Text = "";
            SearchPaymentDatePicker.SelectedDate = null;
            SearchAmountTextBox.Text = "";
            SearchMethodTextBox.Text = "";
            SearchNoteTextBox.Text = "";
            LoadPayments();
        }

        private void ClearInput()
        {
            CustomerIdTextBox.Text = "";
            PaymentDatePicker.SelectedDate = DateTime.Now;
            AmountTextBox.Text = "";
            MethodTextBox.Text = "";
            NoteTextBox.Text = "";
            _selectedPayment = new();
            PaymentDataGrid.UnselectAll();
        }
    }
} 