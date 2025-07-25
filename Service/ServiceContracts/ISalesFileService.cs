using Service.Data;

namespace Service.ServiceContracts
{
    public interface ISalesFileService
    {
        Task<GridData<SalesFileGridData>> GetAllPaginated(DTParameters param);
        Task<GridData<SalesFileGridData>> GetForDisplay(int offset = 0, int pageSize = 8);
        Task<bool> Create(Stream pdfStream, SalesFileData data);
        Task<string> GetFileName(long id);
        Task<bool> UpdateOrder(long id, int order);
        Task<bool> Delete(long id);
    }
}
