using System.ComponentModel.DataAnnotations.Schema;

namespace App.DataAccessLayer.EntityModel.SQL.Model
{
    public class Order : BaseEntity<int>
    {
        public string? SalesmanId { get; set; }
        [ForeignKey(nameof(SalesmanId))]    
        public ApplicationUser? Salesman { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;
        public List<OrderItem>? OrderItems { get; set; }
        public decimal TotalAmount { get; set; }
    }
}