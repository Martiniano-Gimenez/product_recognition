using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Model.Domain;
using Model.Repositories;

namespace Persistance.Context
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ShoppingCartContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public IUserRepository UserRepository { get; set; }
        public IProductRepository ProductRepository { get; set; }
        public IOrderRepository OrderRepository { get; set; }
        public IClientRepository ClientRepository { get; set; }
        public IProductImageRepository ProductImageRepository { get; set; }
        public ICategoryRepository CategoryRepository { get; set; }
        public IDepositMovementRepository DepositMovementRepository { get; set; }

        public UnitOfWork(ShoppingCartContext context,
                         IHttpContextAccessor httpContextAccessor, 
                         IUserRepository userRepository,
                         IProductRepository productRepository,
                         IOrderRepository orderRepository,
                         IClientRepository clientRepository,
                         IProductImageRepository salesFileRepository,
                         ICategoryRepository categoryRepository,
                         IDepositMovementRepository depositMovementRepository)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            UserRepository = userRepository;
            ProductRepository = productRepository;
            OrderRepository = orderRepository;
            ClientRepository = clientRepository;
            ProductImageRepository = salesFileRepository;
            CategoryRepository = categoryRepository;
            DepositMovementRepository = depositMovementRepository;
        }

        public async Task<bool> SaveChangesAsync()
        {
            SetAudit();
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task BeginTransactionAsync()
        {
            await _context.Database.BeginTransactionAsync();
        }

        public bool IsTransactionOpened()
        {
            return _context.Database.CurrentTransaction is not null;
        }

        public async Task CommitAsync()
        {
            await _context.Database.CommitTransactionAsync();
        }
        public async Task RollbackAsync()
        {
            await _context.Database.RollbackTransactionAsync();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _context.Dispose();
            }
        }

        private void SetAudit()
        {

            var userName = _httpContextAccessor?.HttpContext?.User?.Identity?.Name ?? "SISTEMA";
            var date = DateTime.Now;

            var activableEntries = _context.ChangeTracker
                                       .Entries<Auditable>()
                                       .Where(x => x.State != EntityState.Unchanged);

            foreach (var activableEntry in activableEntries)
            {
                var entity = activableEntry.Entity;
                if (activableEntry.State == EntityState.Added)
                {
                    entity.CreationUser = userName;
                    entity.CreationDate = date;
                }
                else if (activableEntry.State == EntityState.Modified)
                {
                    entity.LastUpdateUser = userName;
                    entity.LastUpdateDate = date;
                }
            }
        }
    }
}
