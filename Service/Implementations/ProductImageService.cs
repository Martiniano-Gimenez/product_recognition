using Microsoft.EntityFrameworkCore;
using Model.Repositories;
using Service.Data;
using Service.Mappers;
using Service.ServiceContracts;

namespace Service.Implementations
{
    public class ProductImageService : IProductImageService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly string _filesDirectory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "ProductImages");

        public ProductImageService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

            // Si la carpeta no existe, la creo automáticamente
            if (!Directory.Exists(_filesDirectory))
                Directory.CreateDirectory(_filesDirectory);
        }

        public async Task<GridData<ProductImageGridData>> GetAllPaginated(long productId)
        {
            var productImages = _unitOfWork.ProductImageRepository
                .GetFilteredByPage().Where(pi => pi.ProductId == productId)
                .Select(ProductImageMappingExtensions.MapForGridData());

            var productImagesPaginated = await _unitOfWork.ProductImageRepository
                .GetPagedListAsync(productImages, 0, await productImages.CountAsync());

            return productImagesPaginated.MapToPagGridData();
        }

        public async Task<GridData<ProductImageGridData>> GetForDisplay(int offset = 0, int pageSize = 8)
        {
            var productImages = _unitOfWork.ProductImageRepository
                .GetAll()
                .Select(ProductImageMappingExtensions.MapForGridData());

            var productImagesPaginated = await _unitOfWork.ProductImageRepository
                .GetPagedListAsync(productImages, offset, pageSize);

            return productImagesPaginated.MapToPagGridData();
        }

        public async Task<bool> Create(Stream fileStreamInput, ProductImageData data, string fileName)
        {
            await _unitOfWork.BeginTransactionAsync();

            var entity = data.MapToEntity();
            await _unitOfWork.ProductImageRepository.CreateAsync(entity);

            if (!await _unitOfWork.SaveChangesAsync())
                return false;

            try
            {
                var extension = Path.GetExtension(fileName);
                var fileNameFinal = $"{entity.Id}{extension}";
                var path = Path.Combine(_filesDirectory, fileNameFinal);

                using var fileStream = new FileStream(path, FileMode.Create, FileAccess.Write);
                await fileStreamInput.CopyToAsync(fileStream);

                // 👉 Guardar en BD la ruta relativa (accesible desde el navegador)
                entity.Path = $"/ProductImages/{fileNameFinal}";

                if (!await _unitOfWork.SaveChangesAsync())
                    return false;

                await _unitOfWork.CommitAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> Delete(long id)
        {
            await _unitOfWork.BeginTransactionAsync();

            var entity = await _unitOfWork.ProductImageRepository
                .GetByCondition(sf => sf.Id == id)
                .FirstOrDefaultAsync();

            if (entity is null)
                return false;

            _unitOfWork.ProductImageRepository.Remove(entity);

            if (!await _unitOfWork.SaveChangesAsync())
                return false;

            try
            {
                // Convertir la ruta relativa en ruta física
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", entity.Path.TrimStart('/'));

                if (File.Exists(filePath))
                    File.Delete(filePath);

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
