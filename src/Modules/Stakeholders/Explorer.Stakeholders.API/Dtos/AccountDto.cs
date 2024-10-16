using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Stakeholders.API.Dtos;

public class AccountDto
{
    public long Id { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public int Role { get; set; }
    public bool IsBlocked { get; set; }
}
