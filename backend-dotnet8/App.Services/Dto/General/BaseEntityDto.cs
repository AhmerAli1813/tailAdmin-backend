namespace App.Services.Dto.General;

public class BaseEntityDto
{

    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
    public bool IsActive { get; set; } = true;
    public bool IsDeleted { get; set; } = false;
}
