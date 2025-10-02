namespace Model.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        IUserRepository UserRepository { get; set; }
        IProductRepository ProductRepository { get; set; }
        IOrderRepository OrderRepository { get; set; }
        IClientRepository ClientRepository { get; set; }
        IProductImageRepository ProductImageRepository { get; set; }
        ICategoryRepository CategoryRepository { get; set; }
        IDepositMovementRepository DepositMovementRepository { get; set; }

        Task<bool> SaveChangesAsync();
        Task BeginTransactionAsync();
        Task CommitAsync();
    }
}
