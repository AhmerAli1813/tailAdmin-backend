using System.ComponentModel.DataAnnotations.Schema;

namespace App.DataAccessLayer.EntityModel.SQL.Model
{
    public class Stock : BaseEntity<int>
    {
        public int? ProductId { get; set; }
        [ForeignKey(nameof(ProductId))]
        public Product? Product { get; set; }
        public int Quantity { get; set; }
        public DateTime LastUpdated { get; set; } = DateTime.UtcNow;
    }
}