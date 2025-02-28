using Microsoft.AspNetCore.Http;

namespace App.Services.Dto.General;

public class ResponseDto
{
    public bool IsSucceed { get; set; } = true;
    public int StatusCode { get; set; } = StatusCodes.Status200OK;
    public string? Message { get; set; } = "Successfully";
    public List<string>? Error { get; set; }
    public object? Result { get; set; }
}
