using Service.Data;

namespace Service.ServiceContracts
{
    public interface IProductService
    {
        Task<GridData<ProductGridData>> GetAllPaginatedForShoppingCart(DTParameters param);
        Task<GridData<ProductGridData>> GetAllPaginated(DTParameters param);
        Task<ProductData> GetById(long id);
        Task<ProductViewData> GetByIdForView(long id);
        Task<bool> Edit(ProductData data);
        Task<ShoppingCartProductDetailData> GetShoppingCartProductDetailData(long productId);
        Task<decimal> GetPriceByQuantity(long productId, int quantity);
        Task<List<KeyValueData>> GetAllSelectableByTerm(string term);
    }
}
