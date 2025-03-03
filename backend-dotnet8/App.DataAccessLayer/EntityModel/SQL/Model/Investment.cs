using System.ComponentModel.DataAnnotations.Schema;

namespace App.DataAccessLayer.EntityModel.SQL.Model
{
    public class Investment : BaseEntity<int>
    {
        [ForeignKey(nameof(InvestorId))]
        public ApplicationUser? Investor { get; set; }
        public string? InvestorId { get; set; }
        public int? ProductId { get; set; }
        [ForeignKey(nameof(ProductId))]
        public Product? Product { get; set; }
        public decimal? Amount { get; set; }
        public decimal? ProfitPercentage { get; set; }
    }
}