using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Model.Domain;
using Model.Repositories;
using OfficeOpenXml;
using Service.Data;
using Service.Mappers;
using Service.ServiceContracts;
using System.Data.Odbc;

namespace Service.Implementations
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProductService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;   
        }

        public async Task<GridData<ProductGridData>> GetAllPaginatedForShoppingCart(DTParameters param)
        {
            var groupId = param.GetColumnSearchDataAsLong(0);
            var categoryId = param.GetColumnSearchDataAsLong(1);

            var products = _unitOfWork.ProductRepository
                .GetFilteredByPage(param.SearchValue, param.OrderBy, param.SortDirection(DTOrderDir.ASC), groupId, categoryId)
                .Select(ProductMappingExtensions.MapForGridDataForShoppingCart());

            var productsPaginated = await _unitOfWork.ProductRepository.GetPagedListAsync(products, param.Start, param.Length);
            return productsPaginated.MapToPagGridDataForShoppingCart();
        }

        public async Task<GridData<ProductGridData>> GetAllPaginated(DTParameters param)
        {
            var products = _unitOfWork.ProductRepository
                .GetFilteredByPage(param.SearchValue, param.OrderBy, param.SortDirection(DTOrderDir.ASC), null, null)
                .Select(ProductMappingExtensions.MapForGridData());

            var productsPaginated = await _unitOfWork.ProductRepository.GetPagedListAsync(products, param.Start, param.Length);
            return productsPaginated.MapToPagGridData();
        }

        public async Task<ProductData> GetById(long id)
        {
            return await _unitOfWork.ProductRepository.GetByCondition(c => c.Id == id, true)
                .Select(ProductMappingExtensions.MapToData()).FirstOrDefaultAsync() 
                ?? throw new BusinessException("El artículo no existe");
        }

        public async Task<ProductViewData> GetByIdForView(long id)
        {
            return await _unitOfWork.ProductRepository.GetByCondition(o => o.Id == id, true)
                .Select(ProductMappingExtensions.MapToViewData()).FirstOrDefaultAsync() 
                ?? throw new BusinessException("El pedido no existe");
        }

        public async Task<bool> Edit(ProductData data)
        {
            var entity = await _unitOfWork.ProductRepository.GetByCondition(c => c.Id == data.Id).FirstOrDefaultAsync()
                ?? throw new BusinessException("El artículo no existe");

            entity = data.MapToEntity(entity);

            return await _unitOfWork.SaveChangesAsync();
        }

        public async Task<ShoppingCartProductDetailData> GetShoppingCartProductDetailData(long productId)
        {
            return await _unitOfWork.ProductRepository.GetByCondition(p => p.Id == productId)
                .Select(ProductMappingExtensions.MapToShoppingCartProductDetailData()).FirstOrDefaultAsync() ?? new ShoppingCartProductDetailData();
        }

        public async Task<decimal> GetPriceByQuantity(long productId, int quantity)
        {
            return 0;
        }

        public async Task<List<KeyValueData>> GetAllSelectableByTerm(string term)
        {
            return await _unitOfWork.ProductRepository.GetByTerm(term)
                .Select(ProductMappingExtensions.MapForGetByTerm()).ToListAsync();
        }
    }
}
