using Microsoft.EntityFrameworkCore;
using Model.Repositories;
using Persistance.Context;
using Persistance.Repositories;
using Service.Implementations;
using Service.ServiceContracts;

namespace ShoppingCart.ProgramConfiguration
{
    public static class ServicesConfiguration
    {
        public static void ConfigureServices(this IServiceCollection services, IConfiguration configuration)
        {
            #region DbContext
            services.AddDbContext<ShoppingCartContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("csShoppingCart"), sqlOptions => sqlOptions.UseQuerySplittingBehavior(QuerySplittingBehavior.SingleQuery));
                options.UseProjectables();
            }
            , ServiceLifetime.Scoped);
            #endregion

            #region Repositories
            services.AddTransient<IUnitOfWork, UnitOfWork>();
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IProductRepository, ProductRepository>();
            services.AddTransient<ICartRepository, CartRepository>();
            services.AddTransient<ICartDetailRepository, CartDetailRepository>();
            services.AddTransient<IOrderRepository, OrderRepository>();
            services.AddTransient<IOrderHistoryRepository, OrderHistoryRepository>();
            services.AddTransient<IClientRepository, ClientRepository>();
            services.AddTransient<ISellerRepository, SellerRepository>();
            services.AddTransient<ISalesFileRepository, SalesFileRepository>();
            services.AddTransient<IDownloadFileRepository, DownloadFileRepository>();
            services.AddTransient<IGroupRepository, GroupRepository>();
            services.AddTransient<ICategoryRepository, CategoryRepository>();
            #endregion

            #region Services
            services.AddTransient<IRoleLevelService, RoleLevelService>();
            services.AddTransient<IRecaptchaService, RecaptchaService>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IProductService, ProductService>();
            services.AddTransient<ICartService, CartService>();
            services.AddTransient<IOrderService, OrderService>();
            services.AddTransient<IClientService, ClientService>();
            services.AddTransient<ISellerService, SellerService>();
            services.AddTransient<ISalesFileService, SalesFileService>();
            services.AddTransient<IDownloadFileService, DownloadFileService>();
            services.AddTransient<IGroupService, GroupService>();
            services.AddTransient<ICategoryService, CategoryService>();
            #endregion
        }
    }
}
