using System.ComponentModel.DataAnnotations.Schema;

namespace App.DataAccessLayer.EntityModel.SQL.Model;

public class Product : BaseEntity<int>
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public decimal? Price { get; set; }
    public int StockQuantity { get; set; }
    public decimal InvestmentAmount { get; set; }
    public decimal ProfitPercentage { get; set; }
    [ForeignKey(nameof(CategoryId))]
    public Category? Category { get; set; }
    public int? CategoryId { get; set; }
}