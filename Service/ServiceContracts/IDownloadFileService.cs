using Service.Data;

namespace Service.ServiceContracts
{
    public interface IDownloadFileService
    {
        Task<GridData<DownloadFileGridData>> GetAllPaginated(DTParameters param, short fileExtensionId);
        Task<DownloadFileDisplayData> GetForDisplay(int offsetPdf = 0, int pageSizePdf = 6, int offsetXlsx = 0, int pageSizeXlsx = 6);
        Task<bool> Create(Stream pdfStream, DownloadFileData data);
        Task<bool> UpdateOrder(long id, int order);
        Task<bool> Delete(long id);
        Task<DownloadFileData> GetFileNameAndExtension(long id);
    }
}
