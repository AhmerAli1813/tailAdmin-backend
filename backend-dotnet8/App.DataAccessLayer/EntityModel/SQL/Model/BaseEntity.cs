using System.ComponentModel.DataAnnotations;

namespace App.DataAccessLayer.EntityModel.SQL.Model;

public class BaseEntity<TID>
{
    [Key]
    public TID Id { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
    public bool IsActive { get; set; } = true;
    public bool IsDeleted { get; set; } = false;
}
