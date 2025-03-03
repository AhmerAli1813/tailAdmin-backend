using System.ComponentModel.DataAnnotations.Schema;

namespace App.DataAccessLayer.EntityModel.SQL.Model;

public class Dispatch : BaseEntity<int>
{
    public int? OrderId { get; set; }
    [ForeignKey(nameof(OrderId))]
    public Order? Order { get; set; }
    public DateTime DispatchDate { get; set; } = DateTime.UtcNow;
    public DispatchEnum? Status { get; set; } // Pending, Dispatched, Delivered
}
public enum DispatchEnum
{
    Pending,
    Dispatched,
    Delivered
}