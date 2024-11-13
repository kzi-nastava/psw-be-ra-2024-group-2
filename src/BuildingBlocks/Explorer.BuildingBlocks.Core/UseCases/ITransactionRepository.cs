namespace Explorer.BuildingBlocks.Core.UseCases;

public interface ITransactionRepository
{
    void BeginTransaction();
    void CommitTransaction();
    bool HasActiveTransacation();
    void RollbackTransaction();
}
