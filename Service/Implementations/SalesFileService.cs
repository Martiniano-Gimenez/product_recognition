using Microsoft.EntityFrameworkCore;
using Model.Repositories;
using Service.Data;
using Service.Mappers;
using Service.ServiceContracts;

namespace Service.Implementations
{
    public class SalesFileService : ISalesFileService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly string _filesDirectory = Path.Combine(Directory.GetCurrentDirectory(), "Files", "SalesFiles", "PDF");

        public SalesFileService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<GridData<SalesFileGridData>> GetAllPaginated(DTParameters param)
        {
            var salesFiles = _unitOfWork.SalesFileRepository
                .GetFilteredByPage(param.SearchValue, param.OrderBy, param.SortDirection(DTOrderDir.DESC))
                .Select(SalesFileMappingExtensions.MapForGridData());

            var salesFilesPaginated = await _unitOfWork.SalesFileRepository.GetPagedListAsync(salesFiles, param.Start, param.Length);
            return salesFilesPaginated.MapToPagGridData();
        }

        public async Task<GridData<SalesFileGridData>> GetForDisplay(int offset = 0, int pageSize = 8)
        {
            var salesFiles = _unitOfWork.SalesFileRepository.GetAll()
               .Select(SalesFileMappingExtensions.MapForGridData());

            var salesFilesPaginated = await _unitOfWork.SalesFileRepository.GetPagedListAsync(salesFiles, offset, pageSize);

            salesFilesPaginated.Results = salesFilesPaginated.Results.OrderByDescending(o => o.Order).ToList();

            return salesFilesPaginated.MapToPagGridData();
        }

        public async Task<bool> Create(Stream pdfStream, SalesFileData data)
        {
            await _unitOfWork.BeginTransactionAsync();

            var entity = data.MapToEntity();
            await _unitOfWork.SalesFileRepository.CreateAsync(entity);

            if(!await _unitOfWork.SaveChangesAsync())
                return false;

            try
            {
                var path = Path.Combine(_filesDirectory, $"{entity.Id}.pdf");
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

        public async Task<string> GetFileName(long id)
        {
            return await _unitOfWork.SalesFileRepository.GetByCondition(sf => sf.Id == id)
                .Select(sf => sf.Name).FirstOrDefaultAsync() ?? string.Empty;
        }

        public async Task<bool> UpdateOrder(long id, int order)
        {
            var entity = await _unitOfWork.SalesFileRepository.GetByCondition(sf => sf.Id == id).FirstOrDefaultAsync();

            if(entity is null)
                return false;

            entity.Order = order;

            return await _unitOfWork.SaveChangesAsync();
        }

        public async Task<bool> Delete(long id)
        {
            await _unitOfWork.BeginTransactionAsync();
            var entity = await _unitOfWork.SalesFileRepository.GetByCondition(sf => sf.Id == id).FirstOrDefaultAsync();

            if (entity is null)
                return false;

            _unitOfWork.SalesFileRepository.Remove(entity);

            if (!await _unitOfWork.SaveChangesAsync())
                return false;

            try
            {
                var path = Path.Combine(_filesDirectory, $"{entity.Id}.pdf");
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
