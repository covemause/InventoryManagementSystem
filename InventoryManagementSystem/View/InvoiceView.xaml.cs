using System;
using System.Collections.Generic;
using System.Windows;
using InventoryManagementSystem.Controller;
using InventoryManagementSystem.DataBase.Model;

namespace InventoryManagementSystem.View
{
    public partial class InvoiceView : Window
    {
        private readonly InvoiceController _controller = new InvoiceController();
        private Invoice _selectedInvoice = null;

        public InvoiceView()
        {
            InitializeComponent();
            LoadInvoices();
        }

        private void LoadInvoices()
        {
            InvoiceDataGrid.ItemsSource = _controller.GetAllInvoices();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            var invoice = new Invoice
            {
                SlipType = SlipTypeTextBox.Text,
                CustomerId = int.TryParse(CustomerIdTextBox.Text, out int cid) ? cid : 0,
                Quantity = int.TryParse(QuantityTextBox.Text, out int qty) ? qty : 0,
                Amount = decimal.TryParse(AmountTextBox.Text, out decimal amt) ? amt : 0,
                TaxRate = decimal.TryParse(TaxRateTextBox.Text, out decimal tax) ? tax : 0,
                Note = NoteTextBox.Text,
                CreatedAt = DateTime.Now,
                CreatedBy = Environment.UserName,
                UpdatedAt = DateTime.Now,
                UpdatedBy = Environment.UserName
            };
            _controller.AddInvoice(invoice);
            LoadInvoices();
            ClearInput();
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedInvoice == null) return;
            _selectedInvoice.SlipType = SlipTypeTextBox.Text;
            _selectedInvoice.CustomerId = int.TryParse(CustomerIdTextBox.Text, out int cid) ? cid : 0;
            _selectedInvoice.Quantity = int.TryParse(QuantityTextBox.Text, out int qty) ? qty : 0;
            _selectedInvoice.Amount = decimal.TryParse(AmountTextBox.Text, out decimal amt) ? amt : 0;
            _selectedInvoice.TaxRate = decimal.TryParse(TaxRateTextBox.Text, out decimal tax) ? tax : 0;
            _selectedInvoice.Note = NoteTextBox.Text;
            _selectedInvoice.UpdatedAt = DateTime.Now;
            _selectedInvoice.UpdatedBy = Environment.UserName;
            _controller.UpdateInvoice(_selectedInvoice);
            LoadInvoices();
            ClearInput();
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedInvoice == null) return;
            _controller.DeleteInvoice(_selectedInvoice.Id);
            LoadInvoices();
            ClearInput();
        }

        private void InvoiceDataGrid_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            _selectedInvoice = InvoiceDataGrid.SelectedItem as Invoice;
            if (_selectedInvoice != null)
            {
                SlipTypeTextBox.Text = _selectedInvoice.SlipType ?? "";
                CustomerIdTextBox.Text = _selectedInvoice.CustomerId.ToString();
                QuantityTextBox.Text = _selectedInvoice.Quantity.ToString();
                AmountTextBox.Text = _selectedInvoice.Amount.ToString();
                TaxRateTextBox.Text = _selectedInvoice.TaxRate.ToString();
                NoteTextBox.Text = _selectedInvoice.Note ?? "";
            }
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            string slipType = SearchSlipTypeTextBox.Text;
            int? customerId = int.TryParse(SearchCustomerIdTextBox.Text, out int cid) ? cid : (int?)null;
            int? quantity = int.TryParse(SearchQuantityTextBox.Text, out int qty) ? qty : (int?)null;
            decimal? amount = decimal.TryParse(SearchAmountTextBox.Text, out decimal amt) ? amt : (decimal?)null;
            decimal? taxRate = decimal.TryParse(SearchTaxRateTextBox.Text, out decimal tax) ? tax : (decimal?)null;
            string note = SearchNoteTextBox.Text;
            InvoiceDataGrid.ItemsSource = _controller.SearchInvoices(slipType, customerId, quantity, amount, taxRate, note);
        }

        private void ClearSearchButton_Click(object sender, RoutedEventArgs e)
        {
            SearchSlipTypeTextBox.Text = "";
            SearchCustomerIdTextBox.Text = "";
            SearchQuantityTextBox.Text = "";
            SearchAmountTextBox.Text = "";
            SearchTaxRateTextBox.Text = "";
            SearchNoteTextBox.Text = "";
            LoadInvoices();
        }

        private void ClearInput()
        {
            SlipTypeTextBox.Text = "";
            CustomerIdTextBox.Text = "";
            QuantityTextBox.Text = "";
            AmountTextBox.Text = "";
            TaxRateTextBox.Text = "";
            NoteTextBox.Text = "";
            _selectedInvoice = null;
            InvoiceDataGrid.UnselectAll();
        }
    }
} 