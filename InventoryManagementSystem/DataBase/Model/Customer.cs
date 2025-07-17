using System;

namespace InventoryManagementSystem.DataBase.Model
{
    public class Customer
    {
        public int Id { get; set; } // 顧客ID
        public string Name { get; set; } = string.Empty; // 氏名
        public string Address { get; set; } = string.Empty; // 住所
        public string Phone { get; set; } = string.Empty; // 電話番号
        public string Email { get; set; } = string.Empty; // メールアドレス
        public DateTime RegisteredDate { get; set; } // 登録日
    }
} 