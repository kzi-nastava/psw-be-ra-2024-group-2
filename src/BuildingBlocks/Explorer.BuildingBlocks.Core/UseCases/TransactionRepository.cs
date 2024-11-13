using Explorer.BuildingBlocks.Core.UseCases;
using Microsoft.EntityFrameworkCore;

namespace Explorer.BuildingBlocks.Infrastructure.Database;

public class TransactionRepository<Context> : ITransactionRepository where Context : DbContext
{
    private readonly Context _dbContext;

    public TransactionRepository(Context dbContext)
    {
        _dbContext = dbContext;
    }
    public void BeginTransaction()
    {
        _dbContext.Database.BeginTransaction();
    }

    public void CommitTransaction()
    {
        _dbContext.Database.CommitTransaction();
    }

    public bool HasActiveTransacation()
    {
        return _dbContext.Database.CurrentTransaction != null;
    }

    public void RollbackTransaction()
    {
        _dbContext.Database.RollbackTransaction();
    }
}
