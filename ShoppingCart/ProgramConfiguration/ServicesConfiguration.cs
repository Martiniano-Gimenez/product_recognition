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
            services.AddTransient<IOrderRepository, OrderRepository>();
            services.AddTransient<IClientRepository, ClientRepository>();
            services.AddTransient<IProductImageRepository, ProductImageRepository>();
            services.AddTransient<ICategoryRepository, CategoryRepository>();
            services.AddTransient<IDepositMovementRepository, DepositMovementRepository>();
            services.AddTransient<IDepositRepository, DepositRepository>();
            #endregion

            #region Services
            services.AddTransient<IRoleLevelService, RoleLevelService>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IProductService, ProductService>();
            services.AddTransient<IOrderService, OrderService>();
            services.AddTransient<IClientService, ClientService>();
            services.AddTransient<IProductImageService, ProductImageService>();
            services.AddTransient<ICategoryService, CategoryService>();
            services.AddTransient<IDepositMovementService, DepositMovementService>();
            #endregion
        }
    }
}
