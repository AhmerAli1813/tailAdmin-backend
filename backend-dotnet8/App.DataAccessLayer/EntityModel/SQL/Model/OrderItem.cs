using System.ComponentModel.DataAnnotations.Schema;

namespace App.DataAccessLayer.EntityModel.SQL.Model
{
    public class OrderItem : BaseEntity<int>
    {
        public int? OrderId { get; set; }
        [ForeignKey(nameof(OrderId))]   
        public Order? Order { get; set; }
        public int? ProductId { get; set; }
        [ForeignKey(nameof(ProductId))]
        public Product? Product { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal Total => Quantity * Price;
    }
}