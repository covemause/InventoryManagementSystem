using System.Collections.Generic;
using InventoryManagementSystem.DataBase.DAO;
using InventoryManagementSystem.DataBase.Model;

namespace InventoryManagementSystem.Controller
{
    public class PaymentController
    {
        private readonly PaymentDao _paymentDao;

        public PaymentController()
        {
            _paymentDao = new PaymentDao();
        }

        public void AddPayment(Payment payment)
        {
            _paymentDao.AddPayment(payment);
        }

        public List<Payment> GetAllPayments()
        {
            return _paymentDao.GetAllPayments();
        }

        public void UpdatePayment(Payment payment)
        {
            _paymentDao.UpdatePayment(payment);
        }

        public void DeletePayment(int id)
        {
            _paymentDao.DeletePayment(id);
        }

        public List<Payment> SearchPayments(int? customerId, System.DateTime? paymentDate, decimal? amount, string method, string note)
        {
            return _paymentDao.SearchPayments(customerId, paymentDate, amount, method, note);
        }
    }
} 