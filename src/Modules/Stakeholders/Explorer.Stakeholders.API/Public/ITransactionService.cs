﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Stakeholders.API.Public;

public interface ITransactionService
{
    void BeginTransaction();
    void CommitTransaction();
    void RollbackTransaction();
}
