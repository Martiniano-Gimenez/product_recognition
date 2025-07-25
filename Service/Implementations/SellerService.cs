using Microsoft.EntityFrameworkCore;
using Model.Repositories;
using Service.Data;
using Service.Mappers;
using Service.ServiceContracts;

namespace Service.Implementations
{
    public class SellerService : ISellerService
    {
        private readonly IUnitOfWork _unitOfWork;

        public SellerService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<KeyValueData>> GetAllSelectable()
        {
            return await _unitOfWork.SellerRepository.GetAll()
                .Select(SellerMappingExtensions.MapToKeyValueData()).ToListAsync();
        }
    }
}
