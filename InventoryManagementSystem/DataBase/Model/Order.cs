using System;

namespace InventoryManagementSystem.DataBase.Model
{
    public class Order
    {
        public int Id { get; set; } // 注文ID
        public int CustomerId { get; set; } // 顧客ID
        public int ProductId { get; set; } // 商品ID（在庫IDと同じ）
        public string ProductName { get; set; } = string.Empty; // 商品名
        public int Quantity { get; set; } // 数量
        public decimal UnitPrice { get; set; } // 単価
        public decimal Amount { get; set; } // 金額
        public DateTime OrderDate { get; set; } // 注文日
        public string Note { get; set; } = string.Empty; // 備考
        public bool IsOnlineOrder { get; set; } // オンライン受注フラグ
        public DateTime CreatedAt { get; set; } // 作成日
        public string CreatedBy { get; set; } = string.Empty; // 作成者
        public DateTime UpdatedAt { get; set; } // 更新日
        public string UpdatedBy { get; set; } = string.Empty; // 更新者
    }
} 