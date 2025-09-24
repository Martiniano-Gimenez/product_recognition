namespace Model.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        IUserRepository UserRepository { get; set; }
        IProductRepository ProductRepository { get; set; }
        ICartRepository CartRepository { get; set; }
        ICartDetailRepository CartDetailRepository { get; set; }
        IOrderRepository OrderRepository { get; set; }
        IClientRepository ClientRepository { get; set; }
        ISellerRepository SellerRepository { get; set; }
        IProductImageRepository ProductImageRepository { get; set; }
        IDownloadFileRepository DownloadFileRepository { get; set; }
        ICategoryRepository CategoryRepository { get; set; }
        IGroupRepository GroupRepository { get; set; }

        Task<bool> SaveChangesAsync();
        Task BeginTransactionAsync();
        Task CommitAsync();
    }
}
