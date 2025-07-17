using System.Collections.Generic;
using InventoryManagementSystem.DataBase.DAO;
using InventoryManagementSystem.DataBase.Model;

namespace InventoryManagementSystem.Controller
{
    public class InvoiceController
    {
        private readonly InvoiceDao _invoiceDao;

        public InvoiceController()
        {
            _invoiceDao = new InvoiceDao();
        }

        public void AddInvoice(Invoice invoice)
        {
            _invoiceDao.AddInvoice(invoice);
        }

        public List<Invoice> GetAllInvoices()
        {
            return _invoiceDao.GetAllInvoices();
        }

        public void UpdateInvoice(Invoice invoice)
        {
            _invoiceDao.UpdateInvoice(invoice);
        }

        public void DeleteInvoice(int id)
        {
            _invoiceDao.DeleteInvoice(id);
        }

        public List<Invoice> SearchInvoices(string slipType, int? customerId, int? quantity, decimal? amount, decimal? taxRate, string note)
        {
            return _invoiceDao.SearchInvoices(slipType, customerId, quantity, amount, taxRate, note);
        }
    }
} 