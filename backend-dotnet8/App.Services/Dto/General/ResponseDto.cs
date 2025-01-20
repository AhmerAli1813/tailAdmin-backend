namespace App.Services.Dto.General;

public class ResponseDto
{
    public bool IsSucceed { get; set; }
    public int StatusCode { get; set; }
    public string? Message { get; set; }
}
