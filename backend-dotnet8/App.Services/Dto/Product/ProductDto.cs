using App.Services.Dto.General;
using System.ComponentModel.DataAnnotations;

namespace App.Services.Dto.Product;

public class ProductDto
{
    public string? Id { get; set; }
    [Required(ErrorMessage = "Product Name is Required ")]
    public string? Name { get; set; }
}
public class ProductListDto : BaseEntityDto
{
    public string? Id { get; set; }
    public string? Name { get; set; }

}
public class ProductActiveDto
{
    [Required(ErrorMessage = "Id is Required ")]

    public string? EncryptId { get; set; }
    [Required(ErrorMessage = " Active is Required ")]

    public bool IsActive { get; set; }
}