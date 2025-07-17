using System;
using System.Collections.Generic;
using System.Data.SQLite;
using InventoryManagementSystem.DataBase.Model;

namespace InventoryManagementSystem.DataBase.DAO
{
    public class InvoiceDao
    {
        public InvoiceDao()
        {
            CreateTableIfNotExists();
        }

        private void CreateTableIfNotExists()
        {
            using (var conn = Database.GetConnection())
            {
                conn.Open();
                string sql = @"CREATE TABLE IF NOT EXISTS Invoices (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    SlipType TEXT NOT NULL,
                    CustomerId INTEGER NOT NULL,
                    Quantity INTEGER NOT NULL,
                    Amount REAL NOT NULL,
                    TaxRate REAL NOT NULL,
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

        public void AddInvoice(Invoice invoice)
        {
            using (var conn = Database.GetConnection())
            {
                conn.Open();
                string sql = "INSERT INTO Invoices (SlipType, CustomerId, Quantity, Amount, TaxRate, Note, CreatedAt, CreatedBy, UpdatedAt, UpdatedBy) VALUES (@SlipType, @CustomerId, @Quantity, @Amount, @TaxRate, @Note, @CreatedAt, @CreatedBy, @UpdatedAt, @UpdatedBy)";
                using (var cmd = new SQLiteCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@SlipType", invoice.SlipType);
                    cmd.Parameters.AddWithValue("@CustomerId", invoice.CustomerId);
                    cmd.Parameters.AddWithValue("@Quantity", invoice.Quantity);
                    cmd.Parameters.AddWithValue("@Amount", invoice.Amount);
                    cmd.Parameters.AddWithValue("@TaxRate", invoice.TaxRate);
                    cmd.Parameters.AddWithValue("@Note", invoice.Note);
                    cmd.Parameters.AddWithValue("@CreatedAt", invoice.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss"));
                    cmd.Parameters.AddWithValue("@CreatedBy", invoice.CreatedBy);
                    cmd.Parameters.AddWithValue("@UpdatedAt", invoice.UpdatedAt.ToString("yyyy-MM-dd HH:mm:ss"));
                    cmd.Parameters.AddWithValue("@UpdatedBy", invoice.UpdatedBy);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public List<Invoice> GetAllInvoices()
        {
            var list = new List<Invoice>();
            using (var conn = Database.GetConnection())
            {
                conn.Open();
                string sql = "SELECT * FROM Invoices";
                using (var cmd = new SQLiteCommand(sql, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new Invoice
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            SlipType = reader["SlipType"].ToString(),
                            CustomerId = Convert.ToInt32(reader["CustomerId"]),
                            Quantity = Convert.ToInt32(reader["Quantity"]),
                            Amount = Convert.ToDecimal(reader["Amount"]),
                            TaxRate = Convert.ToDecimal(reader["TaxRate"]),
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

        public void UpdateInvoice(Invoice invoice)
        {
            using (var conn = Database.GetConnection())
            {
                conn.Open();
                string sql = "UPDATE Invoices SET SlipType=@SlipType, CustomerId=@CustomerId, Quantity=@Quantity, Amount=@Amount, TaxRate=@TaxRate, Note=@Note, CreatedAt=@CreatedAt, CreatedBy=@CreatedBy, UpdatedAt=@UpdatedAt, UpdatedBy=@UpdatedBy WHERE Id=@Id";
                using (var cmd = new SQLiteCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@SlipType", invoice.SlipType);
                    cmd.Parameters.AddWithValue("@CustomerId", invoice.CustomerId);
                    cmd.Parameters.AddWithValue("@Quantity", invoice.Quantity);
                    cmd.Parameters.AddWithValue("@Amount", invoice.Amount);
                    cmd.Parameters.AddWithValue("@TaxRate", invoice.TaxRate);
                    cmd.Parameters.AddWithValue("@Note", invoice.Note);
                    cmd.Parameters.AddWithValue("@CreatedAt", invoice.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss"));
                    cmd.Parameters.AddWithValue("@CreatedBy", invoice.CreatedBy);
                    cmd.Parameters.AddWithValue("@UpdatedAt", invoice.UpdatedAt.ToString("yyyy-MM-dd HH:mm:ss"));
                    cmd.Parameters.AddWithValue("@UpdatedBy", invoice.UpdatedBy);
                    cmd.Parameters.AddWithValue("@Id", invoice.Id);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void DeleteInvoice(int id)
        {
            using (var conn = Database.GetConnection())
            {
                conn.Open();
                string sql = "DELETE FROM Invoices WHERE Id=@Id";
                using (var cmd = new SQLiteCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public List<Invoice> SearchInvoices(string slipType, int? customerId, int? quantity, decimal? amount, decimal? taxRate, string note)
        {
            var list = new List<Invoice>();
            using (var conn = Database.GetConnection())
            {
                conn.Open();
                string sql = "SELECT * FROM Invoices WHERE 1=1";
                if (!string.IsNullOrEmpty(slipType)) sql += " AND SlipType LIKE @SlipType";
                if (customerId.HasValue) sql += " AND CustomerId = @CustomerId";
                if (quantity.HasValue) sql += " AND Quantity = @Quantity";
                if (amount.HasValue) sql += " AND Amount = @Amount";
                if (taxRate.HasValue) sql += " AND TaxRate = @TaxRate";
                if (!string.IsNullOrEmpty(note)) sql += " AND Note LIKE @Note";
                using (var cmd = new SQLiteCommand(sql, conn))
                {
                    if (!string.IsNullOrEmpty(slipType)) cmd.Parameters.AddWithValue("@SlipType", "%" + slipType + "%");
                    if (customerId.HasValue) cmd.Parameters.AddWithValue("@CustomerId", customerId.Value);
                    if (quantity.HasValue) cmd.Parameters.AddWithValue("@Quantity", quantity.Value);
                    if (amount.HasValue) cmd.Parameters.AddWithValue("@Amount", amount.Value);
                    if (taxRate.HasValue) cmd.Parameters.AddWithValue("@TaxRate", taxRate.Value);
                    if (!string.IsNullOrEmpty(note)) cmd.Parameters.AddWithValue("@Note", "%" + note + "%");
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(new Invoice
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                SlipType = reader["SlipType"].ToString(),
                                CustomerId = Convert.ToInt32(reader["CustomerId"]),
                                Quantity = Convert.ToInt32(reader["Quantity"]),
                                Amount = Convert.ToDecimal(reader["Amount"]),
                                TaxRate = Convert.ToDecimal(reader["TaxRate"]),
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