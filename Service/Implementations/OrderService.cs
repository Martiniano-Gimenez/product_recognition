using Microsoft.EntityFrameworkCore;
using Model.Domain;
using Model.Repositories;
using Service.Data;
using Service.Mappers;
using Service.ServiceContracts;

namespace Service.Implementations
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        public OrderService(IUnitOfWork unitOfWork) 
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<GridData<OrderGridData>> GetAllPaginatedForShoppingCart(DTParameters param, long userId)
        {
            var orders = _unitOfWork.OrderRepository
                .GetFilteredByPageForShoppingCart(param.SearchValue, param.OrderBy, param.SortDirection(DTOrderDir.DESC), userId)
                .Select(OrderMappingExtensions.MapForGridData());

            var ordersPaginated = await _unitOfWork.OrderRepository.GetPagedListAsync(orders, param.Start, param.Length);
            return ordersPaginated.MapToPagGridData();
        }

        public async Task<GridData<OrderGridData>> GetAllPaginated(DTParameters param, long userId, short roleId)
        {
            var orders = _unitOfWork.OrderRepository
                .GetFilteredByPage(param.SearchValue, param.OrderBy, param.SortDirection(DTOrderDir.DESC), userId, roleId)
                .Select(OrderMappingExtensions.MapForGridData());

            var ordersPaginated = await _unitOfWork.OrderRepository.GetPagedListAsync(orders, param.Start, param.Length);
            return ordersPaginated.MapToPagGridData();
        }

        public async Task<OrderData> GetById(long id)
        {
            return await _unitOfWork.OrderRepository.GetByCondition(o => o.Id == id)
                .Select(OrderMappingExtensions.MapToOrderData()).FirstOrDefaultAsync() ?? throw new BusinessException("El pedido no existe");
        }

        public async Task<OrderViewData> GetByIdForView(long id)
        {
            return await _unitOfWork.OrderRepository.GetByCondition(o => o.Id == id)
                .Select(OrderMappingExtensions.MapToOrderViewData()).FirstOrDefaultAsync() ?? throw new BusinessException("El pedido no existe");
        }

        public async Task<OrderData> AddProductToOrder(OrderData data)
        {
            if (!data.NewProductId.HasValue || !data.NewProductQuantity.HasValue)
                return data;

            if (data.Products.Any(p => p.ProductId == data.NewProductId))
                data.Products.First(p => p.ProductId == data.NewProductId).Quantity += data.NewProductQuantity.Value;
            else
            {
                var product = await _unitOfWork.ProductRepository.GetByCondition(p => p.Id == data.NewProductId, true)
                    .Select(ProductMappingExtensions.MapToOrderDetailData(data.NewProductQuantity.Value)).FirstOrDefaultAsync();
                if (product is not null)
                    data.Products.Add(product);
            }

            return data;
        }

        public async Task<OrderData> AddProductsToOrder(OrderData data, List<long> productIds)
        {
            if (!productIds.Any())
                return data;

            foreach(var productId in productIds)
            {
                if (data.Products.Any(p => p.ProductId == productId))
                    data.Products.First(p => p.ProductId == productId).Quantity += 1;
                else
                {
                    var product = await _unitOfWork.ProductRepository.GetByCondition(p => p.Id == productId, true)
                        .Select(ProductMappingExtensions.MapToOrderDetailData(1)).FirstOrDefaultAsync();
                    if (product is not null)
                        data.Products.Add(product);
                }
            };

            return data;
        }

        public async Task<bool> Create(OrderData data, long userId)
        {
            var canCreateOrder = await _unitOfWork.UserRepository.GetByCondition(u => u.Id == userId, true)
               .Select(u => u.IsActive && (u.RoleId == eRole.Administrator || u.RoleId == eRole.Purchasing)).FirstOrDefaultAsync();

            if (!canCreateOrder)
                throw new BusinessException("No tiene permiso para realizar esta acción");

            if (!data.Products.Any())
                throw new BusinessException("El pedido debe tener al menos un artículo");

            var order = data.MapToEntity();

            await _unitOfWork.OrderRepository.CreateAsync(order);

            return await _unitOfWork.SaveChangesAsync();
        }

        public async Task<bool> Edit(OrderData data, long userId)
        {
            var canEditOrder = await _unitOfWork.UserRepository.GetByCondition(u => u.Id == userId, true)
                .Select(u => u.IsActive && (u.RoleId == eRole.Administrator || u.RoleId == eRole.Purchasing)).FirstOrDefaultAsync();

            if (!canEditOrder)
                throw new BusinessException("No tiene permiso para realizar esta acción");

            if (!data.Products.Any())
                throw new BusinessException("El pedido debe tener al menos un artículo");

            var order = await _unitOfWork.OrderRepository.GetByCondition(o => o.Id == data.Id, false, i => i.Include(o => o.OrderDetails.Where(od => od.IsActive)).ThenInclude(od => od.Product)).FirstOrDefaultAsync()
                ?? throw new BusinessException("El pedido no existe");

            order.Observation = data.Observation;

            order.OrderDetails.ForEach(od => od = od.MapToEntity(data.Products.FirstOrDefault(p => p.Id == od.Id)));

            var newProducts = data.Products.Where(p => !p.Id.HasValue).Select(p => p.MapToEntity()).ToList();
            order.OrderDetails.AddRange(newProducts);

            order.Total = order.OrderDetails.Where(od => od.IsActive).Sum(od => od.Total);

            return await _unitOfWork.SaveChangesAsync();
        }
    }
}
