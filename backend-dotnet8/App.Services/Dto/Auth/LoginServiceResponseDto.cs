﻿namespace App.Services.Dto.Auth;

public class LoginServiceResponseDto
{
    public string NewToken { get; set; }

    // This would be returned to front-end
    public UserInfoResult UserInfo { get; set; }
}
