using App.Services.Dto.General;
using System.ComponentModel.DataAnnotations;

namespace App.Services.Dto.Category;

public class CategoryDto
{
    public string? Id { get; set; }
    [Required(ErrorMessage = "Category Name is Required ")]
    public string? Name { get; set; }
}
public class CategoryListDto : BaseEntityDto
{
    public string? Id { get; set; }
    public string? Name { get; set; }

}
public class CategoryActiveDto
{
    [Required(ErrorMessage = "Id is Required ")]

    public string? EncryptId { get; set; }
    [Required(ErrorMessage = " Active is Required ")]

    public bool IsActive { get; set; }
}