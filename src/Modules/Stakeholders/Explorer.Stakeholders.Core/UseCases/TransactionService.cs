using Explorer.Stakeholders.API.Public;
using Explorer.Stakeholders.Core.Domain.RepositoryInterfaces;

namespace Explorer.Stakeholders.Core.UseCases;

public class TransactionService : ITransactionService
{
    private readonly ITransactionRepository _transactionRepository;

    public TransactionService(ITransactionRepository transactionRepository)
    {
        this._transactionRepository = transactionRepository;
    }
    public void BeginTransaction()
    {
        _transactionRepository.BeginTransaction();
    }

    public void CommitTransaction()
    {
        _transactionRepository.CommitTransaction();
    }

    public void RollbackTransaction()
    {
        _transactionRepository.RollbackTransaction();
    }
}
