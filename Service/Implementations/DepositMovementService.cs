using Microsoft.EntityFrameworkCore;
using Model.Domain;
using Model.Repositories;
using Service.Data;
using Service.Mappers;
using Service.ServiceContracts;

namespace Service.Implementations
{
    public class DepositMovementService : IDepositMovementService
    {
        private readonly IUnitOfWork _unitOfWork;
        public DepositMovementService(IUnitOfWork unitOfWork) 
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<KeyValueData>> GetAllDepositsSelectable()
        {
            return await _unitOfWork.DepositRepository.GetAll()
                .Select(entity => new KeyValueData
                {
                    Key = entity.Id,
                    Value = entity.Identifier + " - " + entity.Description,
                }).ToListAsync();
        }

        public async Task<GridData<DepositMovementGridData>> GetAllPaginated(DTParameters param, long userId, short roleId)
        {
            var DepositMovements = _unitOfWork.DepositMovementRepository
                .GetFilteredByPage(param.SearchValue, param.OrderBy, param.SortDirection(DTOrderDir.DESC), userId, roleId)
                .Select(DepositMovementMappingExtensions.MapForGridData());

            var DepositMovementsPaginated = await _unitOfWork.DepositMovementRepository.GetPagedListAsync(DepositMovements, param.Start, param.Length);
            return DepositMovementsPaginated.MapToPagGridData();
        }

        public async Task<DepositMovementData> GetById(long id)
        {
            return await _unitOfWork.DepositMovementRepository.GetByCondition(o => o.Id == id)
                .Select(DepositMovementMappingExtensions.MapToDepositMovementData()).FirstOrDefaultAsync() ?? throw new BusinessException("El movimiento no existe");
        }

        public async Task<DepositMovementViewData> GetByIdForView(long id)
        {
            return await _unitOfWork.DepositMovementRepository.GetByCondition(o => o.Id == id)
                .Select(DepositMovementMappingExtensions.MapToDepositMovementViewData()).FirstOrDefaultAsync() ?? throw new BusinessException("El movimiento no existe");
        }

        public async Task<DepositMovementData> AddProductToDepositMovement(DepositMovementData data)
        {
            if (!data.NewProductId.HasValue || !data.NewProductQuantity.HasValue)
                return data;

            if (data.Products.Any(p => p.ProductId == data.NewProductId))
                data.Products.First(p => p.ProductId == data.NewProductId).Quantity += data.NewProductQuantity.Value;
            else
            {
                var product = await _unitOfWork.ProductRepository.GetByCondition(p => p.Id == data.NewProductId, true)
                    .Select(ProductMappingExtensions.MapToDepositMovementDetailData(data.NewProductQuantity.Value)).FirstOrDefaultAsync();
                if (product is not null)
                    data.Products.Add(product);
            }

            return data;
        }

        public async Task<bool> Create(DepositMovementData data, long userId)
        {
            var canCreateDepositMovement = await _unitOfWork.UserRepository.GetByCondition(u => u.Id == userId, true)
               .Select(u => u.IsActive && (u.RoleId == eRole.Administrator || u.RoleId == eRole.StockManager)).FirstOrDefaultAsync();

            if (!canCreateDepositMovement)
                throw new BusinessException("No tiene permiso para realizar esta acción");

            if (!data.Products.Any())
                throw new BusinessException("El movimiento debe tener al menos un artículo");

            var DepositMovement = data.MapToEntity();

            await _unitOfWork.DepositMovementRepository.CreateAsync(DepositMovement);

            return await _unitOfWork.SaveChangesAsync();
        }

        public async Task<bool> Edit(DepositMovementData data, long userId)
        {
            var canEditDepositMovement = await _unitOfWork.UserRepository.GetByCondition(u => u.Id == userId, true)
                .Select(u => u.IsActive && (u.RoleId == eRole.Administrator || u.RoleId == eRole.StockManager)).FirstOrDefaultAsync();

            if (!canEditDepositMovement)
                throw new BusinessException("No tiene permiso para realizar esta acción");

            if (!data.Products.Any())
                throw new BusinessException("El movimiento debe tener al menos un artículo");

            var DepositMovement = await _unitOfWork.DepositMovementRepository.GetByCondition(o => o.Id == data.Id, false, i => i.Include(o => o.DepositMovementDetails.Where(od => od.IsActive)).ThenInclude(od => od.Product)).FirstOrDefaultAsync()
                ?? throw new BusinessException("El movimiento no existe");

            DepositMovement.Observation = data.Observation;

            DepositMovement.DepositMovementDetails.ForEach(od => od = od.MapToEntity(data.Products.FirstOrDefault(p => p.Id == od.Id)));

            var newProducts = data.Products.Where(p => !p.Id.HasValue).Select(p => p.MapToEntity()).ToList();
            DepositMovement.DepositMovementDetails.AddRange(newProducts);

            return await _unitOfWork.SaveChangesAsync();
        }

        public async Task<DepositMovementData> AddProductsToMovement(DepositMovementData data, List<long> productIds)
        {
            if (!productIds.Any())
                return data;

            foreach (var productId in productIds)
            {
                if (data.Products.Any(p => p.ProductId == productId))
                    data.Products.First(p => p.ProductId == productId).Quantity += 1;
                else
                {
                    var product = await _unitOfWork.ProductRepository.GetByCondition(p => p.Id == productId, true)
                        .Select(ProductMappingExtensions.MapToDepositMovementDetailData(1)).FirstOrDefaultAsync();
                    if (product is not null)
                        data.Products.Add(product);
                }
            }
            ;

            return data;
        }
    }
}
