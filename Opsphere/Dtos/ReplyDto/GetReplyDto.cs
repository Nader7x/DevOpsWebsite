﻿using Opsphere.Dtos.User;

namespace Opsphere.Dtos.ReplyDto;

public class GetReplyDto
{
    public string? ReplyContent { get; set; }

    public ReplyUserDto? User { get; set; }
}