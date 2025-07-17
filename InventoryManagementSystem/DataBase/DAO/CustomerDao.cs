using System;
using System.Collections.Generic;
using System.Data.SQLite;
using InventoryManagementSystem.DataBase.Model;

namespace InventoryManagementSystem.DataBase.DAO
{
    public class CustomerDao
    {
        public CustomerDao()
        {
            CreateTableIfNotExists();
        }

        private void CreateTableIfNotExists()
        {
            using (var conn = Database.GetConnection())
            {
                conn.Open();
                string sql = @"CREATE TABLE IF NOT EXISTS Customers (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Name TEXT NOT NULL,
                    Address TEXT,
                    Phone TEXT,
                    Email TEXT,
                    RegisteredDate TEXT
                );";
                using (var cmd = new SQLiteCommand(sql, conn))
                {
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void AddCustomer(Customer customer)
        {
            using (var conn = Database.GetConnection())
            {
                conn.Open();
                string sql = "INSERT INTO Customers (Name, Address, Phone, Email, RegisteredDate) VALUES (@Name, @Address, @Phone, @Email, @RegisteredDate)";
                using (var cmd = new SQLiteCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@Name", customer.Name);
                    cmd.Parameters.AddWithValue("@Address", customer.Address);
                    cmd.Parameters.AddWithValue("@Phone", customer.Phone);
                    cmd.Parameters.AddWithValue("@Email", customer.Email);
                    cmd.Parameters.AddWithValue("@RegisteredDate", customer.RegisteredDate.ToString("yyyy-MM-dd"));
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public List<Customer> GetAllCustomers()
        {
            var list = new List<Customer>();
            using (var conn = Database.GetConnection())
            {
                conn.Open();
                string sql = "SELECT * FROM Customers";
                using (var cmd = new SQLiteCommand(sql, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new Customer
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            Name = reader["Name"].ToString(),
                            Address = reader["Address"].ToString(),
                            Phone = reader["Phone"].ToString(),
                            Email = reader["Email"].ToString(),
                            RegisteredDate = DateTime.Parse(reader["RegisteredDate"].ToString())
                        });
                    }
                }
            }
            return list;
        }

        public void UpdateCustomer(Customer customer)
        {
            using (var conn = Database.GetConnection())
            {
                conn.Open();
                string sql = "UPDATE Customers SET Name=@Name, Address=@Address, Phone=@Phone, Email=@Email, RegisteredDate=@RegisteredDate WHERE Id=@Id";
                using (var cmd = new SQLiteCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@Name", customer.Name);
                    cmd.Parameters.AddWithValue("@Address", customer.Address);
                    cmd.Parameters.AddWithValue("@Phone", customer.Phone);
                    cmd.Parameters.AddWithValue("@Email", customer.Email);
                    cmd.Parameters.AddWithValue("@RegisteredDate", customer.RegisteredDate.ToString("yyyy-MM-dd"));
                    cmd.Parameters.AddWithValue("@Id", customer.Id);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void DeleteCustomer(int id)
        {
            using (var conn = Database.GetConnection())
            {
                conn.Open();
                string sql = "DELETE FROM Customers WHERE Id=@Id";
                using (var cmd = new SQLiteCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public List<Customer> SearchCustomers(string name, string phone, string email)
        {
            var list = new List<Customer>();
            using (var conn = Database.GetConnection())
            {
                conn.Open();
                string sql = "SELECT * FROM Customers WHERE 1=1";
                if (!string.IsNullOrEmpty(name)) sql += " AND Name LIKE @Name";
                if (!string.IsNullOrEmpty(phone)) sql += " AND Phone LIKE @Phone";
                if (!string.IsNullOrEmpty(email)) sql += " AND Email LIKE @Email";
                using (var cmd = new SQLiteCommand(sql, conn))
                {
                    if (!string.IsNullOrEmpty(name)) cmd.Parameters.AddWithValue("@Name", "%" + name + "%");
                    if (!string.IsNullOrEmpty(phone)) cmd.Parameters.AddWithValue("@Phone", "%" + phone + "%");
                    if (!string.IsNullOrEmpty(email)) cmd.Parameters.AddWithValue("@Email", "%" + email + "%");
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(new Customer
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                Name = reader["Name"].ToString(),
                                Address = reader["Address"].ToString(),
                                Phone = reader["Phone"].ToString(),
                                Email = reader["Email"].ToString(),
                                RegisteredDate = DateTime.Parse(reader["RegisteredDate"].ToString())
                            });
                        }
                    }
                }
            }
            return list;
        }
    }
} 