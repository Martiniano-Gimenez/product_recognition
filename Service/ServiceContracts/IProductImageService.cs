using Service.Data;

namespace Service.ServiceContracts
{
    public interface IProductImageService
    {
        Task<GridData<ProductImageGridData>> GetAllPaginated(long productId);
        Task<GridData<ProductImageGridData>> GetForDisplay(int offset = 0, int pageSize = 8);
        Task<bool> Create(Stream pdfStream, ProductImageData data, string fileName);
        Task<bool> Delete(long id);
    }
}
