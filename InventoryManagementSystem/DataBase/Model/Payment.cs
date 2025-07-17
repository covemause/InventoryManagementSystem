using System;

namespace InventoryManagementSystem.DataBase.Model
{
    public class Payment
    {
        public int Id { get; set; } // 入金ID
        public int CustomerId { get; set; } // 顧客ID
        public DateTime PaymentDate { get; set; } // 入金日
        public decimal Amount { get; set; } // 金額
        public string Method { get; set; } = string.Empty; // 入金方法
        public string Note { get; set; } = string.Empty; // 備考
        public DateTime CreatedAt { get; set; } // 作成日
        public string CreatedBy { get; set; } = string.Empty; // 作成者
        public DateTime UpdatedAt { get; set; } // 更新日
        public string UpdatedBy { get; set; } = string.Empty; // 更新者
    }
} 