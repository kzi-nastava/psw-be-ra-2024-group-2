using Explorer.BuildingBlocks.Core.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Stakeholders.API.Dtos;

public class AccountDto
{
    public long UserId { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public UserRole Role { get; set; }
    public bool IsBlocked { get; set; }
}
