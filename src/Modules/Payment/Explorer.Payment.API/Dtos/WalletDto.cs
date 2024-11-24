using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Payment.API.Dtos;

public class WalletDto
{
    public long UserId { get; set; }
    public long AdventureCoinsBalance { get; set; }
}
