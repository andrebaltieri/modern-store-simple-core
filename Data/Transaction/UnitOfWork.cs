using ModernStore.Data.Contexts;

namespace ModernStore.Data.Transaction {
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ModernStoreDataContext _context;

        public UnitOfWork(ModernStoreDataContext context)
        {
            _context=context;
        }
        public void Commit()
        {
            _context.SaveChanges();
        }

        public void Rollback()
        {
            // Do Nothing
        }
    }
}