using System.Collections.Generic;
using InventoryManagementSystem.DataBase.DAO;
using InventoryManagementSystem.DataBase.Model;

namespace InventoryManagementSystem.Controller
{
    public class OrderController
    {
        private readonly OrderDao _orderDao;

        public OrderController()
        {
            _orderDao = new OrderDao();
        }

        public void AddOrder(Order order)
        {
            _orderDao.AddOrder(order);
        }

        public List<Order> GetAllOrders()
        {
            return _orderDao.GetAllOrders();
        }

        public void UpdateOrder(Order order)
        {
            _orderDao.UpdateOrder(order);
        }

        public void DeleteOrder(int id)
        {
            _orderDao.DeleteOrder(id);
        }

        public List<Order> SearchOrders(int? customerId, int? productId, string productName, int? quantity, decimal? unitPrice, decimal? amount, System.DateTime? orderDate, string note, bool? isOnlineOrder = null)
        {
            return _orderDao.SearchOrders(customerId, productId, productName, quantity, unitPrice, amount, orderDate, note, isOnlineOrder);
        }
    }
} 