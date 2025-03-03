using System.ComponentModel.DataAnnotations.Schema;

namespace App.DataAccessLayer.EntityModel.SQL.Model
{
    public class PriceManagement : BaseEntity<int>
    {
        public int? ProductId { get; set; }
        [ForeignKey(nameof(ProductId))]
        public Product? Product { get; set; }
        public decimal? Price { get; set; }
        public DateTime? EffectiveDate { get; set; }
        public string? Frequency { get; set; } // Daily, Monthly, Yearly
    }
}