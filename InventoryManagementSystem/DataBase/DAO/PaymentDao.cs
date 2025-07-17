using System;
using System.Collections.Generic;
using System.Data.SQLite;
using InventoryManagementSystem.DataBase.Model;

namespace InventoryManagementSystem.DataBase.DAO
{
    public class PaymentDao
    {
        public PaymentDao()
        {
            CreateTableIfNotExists();
        }

        private void CreateTableIfNotExists()
        {
            using (var conn = Database.GetConnection())
            {
                conn.Open();
                string sql = @"CREATE TABLE IF NOT EXISTS Payments (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    CustomerId INTEGER NOT NULL,
                    PaymentDate TEXT,
                    Amount REAL NOT NULL,
                    Method TEXT,
                    Note TEXT,
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

        public void AddPayment(Payment payment)
        {
            using (var conn = Database.GetConnection())
            {
                conn.Open();
                string sql = "INSERT INTO Payments (CustomerId, PaymentDate, Amount, Method, Note, CreatedAt, CreatedBy, UpdatedAt, UpdatedBy) VALUES (@CustomerId, @PaymentDate, @Amount, @Method, @Note, @CreatedAt, @CreatedBy, @UpdatedAt, @UpdatedBy)";
                using (var cmd = new SQLiteCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@CustomerId", payment.CustomerId);
                    cmd.Parameters.AddWithValue("@PaymentDate", payment.PaymentDate.ToString("yyyy-MM-dd"));
                    cmd.Parameters.AddWithValue("@Amount", payment.Amount);
                    cmd.Parameters.AddWithValue("@Method", payment.Method);
                    cmd.Parameters.AddWithValue("@Note", payment.Note);
                    cmd.Parameters.AddWithValue("@CreatedAt", payment.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss"));
                    cmd.Parameters.AddWithValue("@CreatedBy", payment.CreatedBy);
                    cmd.Parameters.AddWithValue("@UpdatedAt", payment.UpdatedAt.ToString("yyyy-MM-dd HH:mm:ss"));
                    cmd.Parameters.AddWithValue("@UpdatedBy", payment.UpdatedBy);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public List<Payment> GetAllPayments()
        {
            var list = new List<Payment>();
            using (var conn = Database.GetConnection())
            {
                conn.Open();
                string sql = "SELECT * FROM Payments";
                using (var cmd = new SQLiteCommand(sql, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new Payment
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            CustomerId = Convert.ToInt32(reader["CustomerId"]),
                            PaymentDate = DateTime.Parse(reader["PaymentDate"].ToString()),
                            Amount = Convert.ToDecimal(reader["Amount"]),
                            Method = reader["Method"].ToString(),
                            Note = reader["Note"].ToString(),
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

        public void UpdatePayment(Payment payment)
        {
            using (var conn = Database.GetConnection())
            {
                conn.Open();
                string sql = "UPDATE Payments SET CustomerId=@CustomerId, PaymentDate=@PaymentDate, Amount=@Amount, Method=@Method, Note=@Note, CreatedAt=@CreatedAt, CreatedBy=@CreatedBy, UpdatedAt=@UpdatedAt, UpdatedBy=@UpdatedBy WHERE Id=@Id";
                using (var cmd = new SQLiteCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@CustomerId", payment.CustomerId);
                    cmd.Parameters.AddWithValue("@PaymentDate", payment.PaymentDate.ToString("yyyy-MM-dd"));
                    cmd.Parameters.AddWithValue("@Amount", payment.Amount);
                    cmd.Parameters.AddWithValue("@Method", payment.Method);
                    cmd.Parameters.AddWithValue("@Note", payment.Note);
                    cmd.Parameters.AddWithValue("@CreatedAt", payment.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss"));
                    cmd.Parameters.AddWithValue("@CreatedBy", payment.CreatedBy);
                    cmd.Parameters.AddWithValue("@UpdatedAt", payment.UpdatedAt.ToString("yyyy-MM-dd HH:mm:ss"));
                    cmd.Parameters.AddWithValue("@UpdatedBy", payment.UpdatedBy);
                    cmd.Parameters.AddWithValue("@Id", payment.Id);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void DeletePayment(int id)
        {
            using (var conn = Database.GetConnection())
            {
                conn.Open();
                string sql = "DELETE FROM Payments WHERE Id=@Id";
                using (var cmd = new SQLiteCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public List<Payment> SearchPayments(int? customerId, DateTime? paymentDate, decimal? amount, string method, string note)
        {
            var list = new List<Payment>();
            using (var conn = Database.GetConnection())
            {
                conn.Open();
                string sql = "SELECT * FROM Payments WHERE 1=1";
                if (customerId.HasValue) sql += " AND CustomerId = @CustomerId";
                if (paymentDate.HasValue) sql += " AND PaymentDate = @PaymentDate";
                if (amount.HasValue) sql += " AND Amount = @Amount";
                if (!string.IsNullOrEmpty(method)) sql += " AND Method LIKE @Method";
                if (!string.IsNullOrEmpty(note)) sql += " AND Note LIKE @Note";
                using (var cmd = new SQLiteCommand(sql, conn))
                {
                    if (customerId.HasValue) cmd.Parameters.AddWithValue("@CustomerId", customerId.Value);
                    if (paymentDate.HasValue) cmd.Parameters.AddWithValue("@PaymentDate", paymentDate.Value.ToString("yyyy-MM-dd"));
                    if (amount.HasValue) cmd.Parameters.AddWithValue("@Amount", amount.Value);
                    if (!string.IsNullOrEmpty(method)) cmd.Parameters.AddWithValue("@Method", "%" + method + "%");
                    if (!string.IsNullOrEmpty(note)) cmd.Parameters.AddWithValue("@Note", "%" + note + "%");
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(new Payment
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                CustomerId = Convert.ToInt32(reader["CustomerId"]),
                                PaymentDate = DateTime.Parse(reader["PaymentDate"].ToString()),
                                Amount = Convert.ToDecimal(reader["Amount"]),
                                Method = reader["Method"].ToString(),
                                Note = reader["Note"].ToString(),
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