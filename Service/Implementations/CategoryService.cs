using Microsoft.EntityFrameworkCore;
using Model.Repositories;
using Service.Data;
using Service.Mappers;
using Service.ServiceContracts;

namespace Service.Implementations
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        public CategoryService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<KeyValueData>> GetAllSelectable()
        {
            return await _unitOfWork.CategoryRepository.GetAll()
                .Select(CategoryMappingExtensions.MapToKeyValueData()).ToListAsync();
        }
    }
}
