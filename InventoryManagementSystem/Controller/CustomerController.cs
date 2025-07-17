using System.Collections.Generic;
using InventoryManagementSystem.DataBase.DAO;
using InventoryManagementSystem.DataBase.Model;

namespace InventoryManagementSystem.Controller
{
    public class CustomerController
    {
        private readonly CustomerDao _customerDao;

        public CustomerController()
        {
            _customerDao = new CustomerDao();
        }

        public void AddCustomer(Customer customer)
        {
            _customerDao.AddCustomer(customer);
        }

        public List<Customer> GetAllCustomers()
        {
            return _customerDao.GetAllCustomers();
        }

        public void UpdateCustomer(Customer customer)
        {
            _customerDao.UpdateCustomer(customer);
        }

        public void DeleteCustomer(int id)
        {
            _customerDao.DeleteCustomer(id);
        }

        public List<Customer> SearchCustomers(string name, string phone, string email)
        {
            return _customerDao.SearchCustomers(name, phone, email);
        }
    }
} 