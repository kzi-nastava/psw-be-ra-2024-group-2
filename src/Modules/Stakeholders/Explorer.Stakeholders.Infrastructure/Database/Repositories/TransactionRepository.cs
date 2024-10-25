using Explorer.Stakeholders.Core.Domain.RepositoryInterfaces;

namespace Explorer.Stakeholders.Infrastructure.Database.Repositories;

public class TransactionRepository : ITransactionRepository
{
    private readonly StakeholdersContext _dbContext;

    public TransactionRepository(StakeholdersContext dbContext)
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
