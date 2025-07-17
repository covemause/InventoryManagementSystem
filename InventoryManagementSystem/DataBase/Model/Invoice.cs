using System;

namespace InventoryManagementSystem.DataBase.Model
{
    public class Invoice
    {
        public int Id { get; set; } // 伝票ID
        public string SlipType { get; set; } = string.Empty; // 伝票区分（売上伝票・仕入伝票など）
        public int CustomerId { get; set; } // 顧客IDまたは仕入先ID
        public int Quantity { get; set; } // 数量
        public decimal Amount { get; set; } // 金額
        public decimal TaxRate { get; set; } // 消費税率
        public string Note { get; set; } = string.Empty; // 備考
        public DateTime CreatedAt { get; set; } // 作成日
        public string CreatedBy { get; set; } = string.Empty; // 作成者
        public DateTime UpdatedAt { get; set; } // 更新日
        public string UpdatedBy { get; set; } = string.Empty; // 更新者
    }
} 