using Microsoft.EntityFrameworkCore;
using Model.Domain;
using Model.Repositories;
using Service.Data;
using Service.Mappers;
using Service.ServiceContracts;

namespace Service.Implementations
{
    public class DownloadFileService : IDownloadFileService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly string _pdfFilesDirectory = Path.Combine(Directory.GetCurrentDirectory(), "Files", "DownloadFiles", "PDF");
        private readonly string _xlsxFilesDirectory = Path.Combine(Directory.GetCurrentDirectory(), "Files", "DownloadFiles", "XLSX");

        public DownloadFileService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<GridData<DownloadFileGridData>> GetAllPaginated(DTParameters param, short fileExtensionId)
        {
            var downloadFiles = _unitOfWork.DownloadFileRepository
                .GetFilteredByPage(param.SearchValue, param.OrderBy, param.SortDirection(DTOrderDir.DESC), fileExtensionId)
                .Select(DownloadFileMappingExtensions.MapForGridData());

            var downloadFilesPaginated = await _unitOfWork.DownloadFileRepository.GetPagedListAsync(downloadFiles, param.Start, param.Length);
            return downloadFilesPaginated.MapToPagGridData();
        }

        public async Task<DownloadFileDisplayData> GetForDisplay(int offsetPdf = 0, int pageSizePdf = 6, int offsetXlsx = 0, int pageSizeXlsx = 6)
        {
            var downloadFiles = _unitOfWork.DownloadFileRepository.GetAll()
               .Select(DownloadFileMappingExtensions.MapForDisplay());

            var pdfDownloadFilesPaginated = await _unitOfWork.DownloadFileRepository.GetPagedListAsync(downloadFiles.Where(df => df.FileExtension == eFileExtension.Pdf), offsetPdf, pageSizePdf);
            pdfDownloadFilesPaginated.Results = pdfDownloadFilesPaginated.Results.OrderByDescending(o => o.Order).ToList();

            var xlsxDownloadFilesPaginated = await _unitOfWork.DownloadFileRepository.GetPagedListAsync(downloadFiles.Where(df => df.FileExtension == eFileExtension.Xlsx), offsetXlsx, pageSizeXlsx);
            xlsxDownloadFilesPaginated.Results = xlsxDownloadFilesPaginated.Results.OrderByDescending(o => o.Order).ToList();

            return new DownloadFileDisplayData 
            { 
                Pdfs = pdfDownloadFilesPaginated.MapToPagGridData(),
                Xlsxs = xlsxDownloadFilesPaginated.MapToPagGridData()
            };
        }

        public async Task<bool> Create(Stream pdfStream, DownloadFileData data)
        {
            await _unitOfWork.BeginTransactionAsync();

            var entity = data.MapToEntity();
            await _unitOfWork.DownloadFileRepository.CreateAsync(entity);

            if (!await _unitOfWork.SaveChangesAsync())
                return false;

            try
            {
                var path = data.FileExtension == eFileExtension.Pdf
                    ? Path.Combine(_pdfFilesDirectory, $"{entity.Id}.pdf")
                    : Path.Combine(_xlsxFilesDirectory, $"{entity.Id}.xlsx");

                using var fileStream = new FileStream(path, FileMode.Create, FileAccess.Write);
                await pdfStream.CopyToAsync(fileStream);

                await _unitOfWork.CommitAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<DownloadFileData> GetFileNameAndExtension(long id)
        {
            return await _unitOfWork.DownloadFileRepository.GetByCondition(sf => sf.Id == id)
                .Select(sf => new DownloadFileData { Name = sf.Name, FileExtension = sf.FileExtension }).FirstOrDefaultAsync() ?? new DownloadFileData();
        }

        public async Task<bool> UpdateOrder(long id, int order)
        {
            var entity = await _unitOfWork.DownloadFileRepository.GetByCondition(sf => sf.Id == id).FirstOrDefaultAsync();

            if (entity is null)
                return false;

            entity.Order = order;

            return await _unitOfWork.SaveChangesAsync();
        }

        public async Task<bool> Delete(long id)
        {
            await _unitOfWork.BeginTransactionAsync();
            var entity = await _unitOfWork.DownloadFileRepository.GetByCondition(sf => sf.Id == id).FirstOrDefaultAsync();

            if (entity is null)
                return false;

            _unitOfWork.DownloadFileRepository.Remove(entity);

            if (!await _unitOfWork.SaveChangesAsync())
                return false;

            try
            {
                var path = entity.FileExtension == eFileExtension.Pdf
                    ? Path.Combine(_pdfFilesDirectory, $"{entity.Id}.pdf")
                    : Path.Combine(_xlsxFilesDirectory, $"{entity.Id}.xlsx");
                File.Delete(path);

                await _unitOfWork.CommitAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
