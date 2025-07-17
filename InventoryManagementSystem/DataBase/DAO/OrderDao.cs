using System;
using System.Collections.Generic;
using System.Data.SQLite;
using InventoryManagementSystem.DataBase.Model;

namespace InventoryManagementSystem.DataBase.DAO
{
    public class OrderDao
    {
        public OrderDao()
        {
            CreateTableIfNotExists();
        }

        private void CreateTableIfNotExists()
        {
            using (var conn = Database.GetConnection())
            {
                conn.Open();
                string sql = @"CREATE TABLE IF NOT EXISTS Orders (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    CustomerId INTEGER NOT NULL,
                    ProductId INTEGER NOT NULL,
                    ProductName TEXT NOT NULL,
                    Quantity INTEGER NOT NULL,
                    UnitPrice REAL NOT NULL,
                    Amount REAL NOT NULL,
                    OrderDate TEXT,
                    Note TEXT,
                    IsOnlineOrder INTEGER NOT NULL DEFAULT 0,
                    CreatedAt TEXT,
                    CreatedBy TEXT,
                    UpdatedAt TEXT,
                    UpdatedBy TEXT
                );";
                using (var cmd = new SQLiteCommand(sql, conn))
                {
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void AddOrder(Order order)
        {
            using (var conn = Database.GetConnection())
            {
                conn.Open();
                string sql = "INSERT INTO Orders (CustomerId, ProductId, ProductName, Quantity, UnitPrice, Amount, OrderDate, Note, IsOnlineOrder, CreatedAt, CreatedBy, UpdatedAt, UpdatedBy) VALUES (@CustomerId, @ProductId, @ProductName, @Quantity, @UnitPrice, @Amount, @OrderDate, @Note, @IsOnlineOrder, @CreatedAt, @CreatedBy, @UpdatedAt, @UpdatedBy)";
                using (var cmd = new SQLiteCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@CustomerId", order.CustomerId);
                    cmd.Parameters.AddWithValue("@ProductId", order.ProductId);
                    cmd.Parameters.AddWithValue("@ProductName", order.ProductName);
                    cmd.Parameters.AddWithValue("@Quantity", order.Quantity);
                    cmd.Parameters.AddWithValue("@UnitPrice", order.UnitPrice);
                    cmd.Parameters.AddWithValue("@Amount", order.Amount);
                    cmd.Parameters.AddWithValue("@OrderDate", order.OrderDate.ToString("yyyy-MM-dd"));
                    cmd.Parameters.AddWithValue("@Note", order.Note);
                    cmd.Parameters.AddWithValue("@IsOnlineOrder", order.IsOnlineOrder ? 1 : 0);
                    cmd.Parameters.AddWithValue("@CreatedAt", order.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss"));
                    cmd.Parameters.AddWithValue("@CreatedBy", order.CreatedBy);
                    cmd.Parameters.AddWithValue("@UpdatedAt", order.UpdatedAt.ToString("yyyy-MM-dd HH:mm:ss"));
                    cmd.Parameters.AddWithValue("@UpdatedBy", order.UpdatedBy);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public List<Order> GetAllOrders()
        {
            var list = new List<Order>();
            using (var conn = Database.GetConnection())
            {
                conn.Open();
                string sql = "SELECT * FROM Orders";
                using (var cmd = new SQLiteCommand(sql, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new Order
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            CustomerId = Convert.ToInt32(reader["CustomerId"]),
                            ProductId = Convert.ToInt32(reader["ProductId"]),
                            ProductName = reader["ProductName"].ToString(),
                            Quantity = Convert.ToInt32(reader["Quantity"]),
                            UnitPrice = Convert.ToDecimal(reader["UnitPrice"]),
                            Amount = Convert.ToDecimal(reader["Amount"]),
                            OrderDate = DateTime.Parse(reader["OrderDate"].ToString()),
                            Note = reader["Note"].ToString(),
                            IsOnlineOrder = Convert.ToInt32(reader["IsOnlineOrder"]) == 1,
                            CreatedAt = DateTime.Parse(reader["CreatedAt"].ToString()),
                            CreatedBy = reader["CreatedBy"].ToString(),
                            UpdatedAt = DateTime.Parse(reader["UpdatedAt"].ToString()),
                            UpdatedBy = reader["UpdatedBy"].ToString()
                        });
                    }
                }
            }
            return list;
        }

        public void UpdateOrder(Order order)
        {
            using (var conn = Database.GetConnection())
            {
                conn.Open();
                string sql = "UPDATE Orders SET CustomerId=@CustomerId, ProductId=@ProductId, ProductName=@ProductName, Quantity=@Quantity, UnitPrice=@UnitPrice, Amount=@Amount, OrderDate=@OrderDate, Note=@Note, IsOnlineOrder=@IsOnlineOrder, CreatedAt=@CreatedAt, CreatedBy=@CreatedBy, UpdatedAt=@UpdatedAt, UpdatedBy=@UpdatedBy WHERE Id=@Id";
                using (var cmd = new SQLiteCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@CustomerId", order.CustomerId);
                    cmd.Parameters.AddWithValue("@ProductId", order.ProductId);
                    cmd.Parameters.AddWithValue("@ProductName", order.ProductName);
                    cmd.Parameters.AddWithValue("@Quantity", order.Quantity);
                    cmd.Parameters.AddWithValue("@UnitPrice", order.UnitPrice);
                    cmd.Parameters.AddWithValue("@Amount", order.Amount);
                    cmd.Parameters.AddWithValue("@OrderDate", order.OrderDate.ToString("yyyy-MM-dd"));
                    cmd.Parameters.AddWithValue("@Note", order.Note);
                    cmd.Parameters.AddWithValue("@IsOnlineOrder", order.IsOnlineOrder ? 1 : 0);
                    cmd.Parameters.AddWithValue("@CreatedAt", order.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss"));
                    cmd.Parameters.AddWithValue("@CreatedBy", order.CreatedBy);
                    cmd.Parameters.AddWithValue("@UpdatedAt", order.UpdatedAt.ToString("yyyy-MM-dd HH:mm:ss"));
                    cmd.Parameters.AddWithValue("@UpdatedBy", order.UpdatedBy);
                    cmd.Parameters.AddWithValue("@Id", order.Id);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void DeleteOrder(int id)
        {
            using (var conn = Database.GetConnection())
            {
                conn.Open();
                string sql = "DELETE FROM Orders WHERE Id=@Id";
                using (var cmd = new SQLiteCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public List<Order> SearchOrders(int? customerId, int? productId, string productName, int? quantity, decimal? unitPrice, decimal? amount, DateTime? orderDate, string note, bool? isOnlineOrder = null)
        {
            var list = new List<Order>();
            using (var conn = Database.GetConnection())
            {
                conn.Open();
                string sql = "SELECT * FROM Orders WHERE 1=1";
                if (customerId.HasValue) sql += " AND CustomerId = @CustomerId";
                if (productId.HasValue) sql += " AND ProductId = @ProductId";
                if (!string.IsNullOrEmpty(productName)) sql += " AND ProductName LIKE @ProductName";
                if (quantity.HasValue) sql += " AND Quantity = @Quantity";
                if (unitPrice.HasValue) sql += " AND UnitPrice = @UnitPrice";
                if (amount.HasValue) sql += " AND Amount = @Amount";
                if (orderDate.HasValue) sql += " AND OrderDate = @OrderDate";
                if (!string.IsNullOrEmpty(note)) sql += " AND Note LIKE @Note";
                if (isOnlineOrder.HasValue) sql += " AND IsOnlineOrder = @IsOnlineOrder";
                using (var cmd = new SQLiteCommand(sql, conn))
                {
                    if (customerId.HasValue) cmd.Parameters.AddWithValue("@CustomerId", customerId.Value);
                    if (productId.HasValue) cmd.Parameters.AddWithValue("@ProductId", productId.Value);
                    if (!string.IsNullOrEmpty(productName)) cmd.Parameters.AddWithValue("@ProductName", "%" + productName + "%");
                    if (quantity.HasValue) cmd.Parameters.AddWithValue("@Quantity", quantity.Value);
                    if (unitPrice.HasValue) cmd.Parameters.AddWithValue("@UnitPrice", unitPrice.Value);
                    if (amount.HasValue) cmd.Parameters.AddWithValue("@Amount", amount.Value);
                    if (orderDate.HasValue) cmd.Parameters.AddWithValue("@OrderDate", orderDate.Value.ToString("yyyy-MM-dd"));
                    if (!string.IsNullOrEmpty(note)) cmd.Parameters.AddWithValue("@Note", "%" + note + "%");
                    if (isOnlineOrder.HasValue) cmd.Parameters.AddWithValue("@IsOnlineOrder", isOnlineOrder.Value ? 1 : 0);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(new Order
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                CustomerId = Convert.ToInt32(reader["CustomerId"]),
                                ProductId = Convert.ToInt32(reader["ProductId"]),
                                ProductName = reader["ProductName"].ToString(),
                                Quantity = Convert.ToInt32(reader["Quantity"]),
                                UnitPrice = Convert.ToDecimal(reader["UnitPrice"]),
                                Amount = Convert.ToDecimal(reader["Amount"]),
                                OrderDate = DateTime.Parse(reader["OrderDate"].ToString()),
                                Note = reader["Note"].ToString(),
                                IsOnlineOrder = Convert.ToInt32(reader["IsOnlineOrder"]) == 1,
                                CreatedAt = DateTime.Parse(reader["CreatedAt"].ToString()),
                                CreatedBy = reader["CreatedBy"].ToString(),
                                UpdatedAt = DateTime.Parse(reader["UpdatedAt"].ToString()),
                                UpdatedBy = reader["UpdatedBy"].ToString()
                            });
                        }
                    }
                }
            }
            return list;
        }
    }
} 