using System;
using System.Collections.Generic;
using System.Windows;
using InventoryManagementSystem.Controller;
using InventoryManagementSystem.DataBase.Model;

namespace InventoryManagementSystem.View
{
    public partial class CustomerView : Window
    {
        private readonly CustomerController _controller = new CustomerController();
        private Customer _selectedCustomer = null;

        public CustomerView()
        {
            InitializeComponent();
            LoadCustomers();
        }

        private void LoadCustomers()
        {
            CustomerDataGrid.ItemsSource = _controller.GetAllCustomers();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            var customer = new Customer
            {
                Name = NameTextBox.Text,
                Address = AddressTextBox.Text,
                Phone = PhoneTextBox.Text,
                Email = EmailTextBox.Text,
                RegisteredDate = RegisteredDatePicker.SelectedDate ?? DateTime.Now
            };
            _controller.AddCustomer(customer);
            LoadCustomers();
            ClearInput();
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedCustomer == null) return;
            _selectedCustomer.Name = NameTextBox.Text;
            _selectedCustomer.Address = AddressTextBox.Text;
            _selectedCustomer.Phone = PhoneTextBox.Text;
            _selectedCustomer.Email = EmailTextBox.Text;
            _selectedCustomer.RegisteredDate = RegisteredDatePicker.SelectedDate ?? DateTime.Now;
            _controller.UpdateCustomer(_selectedCustomer);
            LoadCustomers();
            ClearInput();
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedCustomer == null) return;
            _controller.DeleteCustomer(_selectedCustomer.Id);
            LoadCustomers();
            ClearInput();
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            string name = SearchNameTextBox.Text;
            string phone = SearchPhoneTextBox.Text;
            string email = SearchEmailTextBox.Text;
            CustomerDataGrid.ItemsSource = _controller.SearchCustomers(name, phone, email);
        }

        private void ClearSearchButton_Click(object sender, RoutedEventArgs e)
        {
            SearchNameTextBox.Text = "";
            SearchPhoneTextBox.Text = "";
            SearchEmailTextBox.Text = "";
            LoadCustomers();
        }

        private void CustomerDataGrid_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            _selectedCustomer = CustomerDataGrid.SelectedItem as Customer;
            if (_selectedCustomer != null)
            {
                NameTextBox.Text = _selectedCustomer.Name ?? "";
                AddressTextBox.Text = _selectedCustomer.Address ?? "";
                PhoneTextBox.Text = _selectedCustomer.Phone ?? "";
                EmailTextBox.Text = _selectedCustomer.Email ?? "";
                RegisteredDatePicker.SelectedDate = _selectedCustomer.RegisteredDate;
            }
        }

        private void ClearInput()
        {
            NameTextBox.Text = "";
            AddressTextBox.Text = "";
            PhoneTextBox.Text = "";
            EmailTextBox.Text = "";
            RegisteredDatePicker.SelectedDate = DateTime.Now;
            _selectedCustomer = null;
            CustomerDataGrid.UnselectAll();
        }
    }
} 