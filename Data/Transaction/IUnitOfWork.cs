namespace ModernStore.Data.Transaction
{
    public interface IUnitOfWork
    {
        void Commit();
        void Rollback();
    }
}