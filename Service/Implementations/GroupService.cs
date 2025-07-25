using Microsoft.EntityFrameworkCore;
using Model.Repositories;
using Service.Data;
using Service.Mappers;
using Service.ServiceContracts;

namespace Service.Implementations
{
    public class GroupService : IGroupService
    {
        private readonly IUnitOfWork _unitOfWork;
        public GroupService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<KeyValueData>> GetAllSelectable()
        {
            return await _unitOfWork.GroupRepository.GetAll()
                .Select(GroupMappingExtensions.MapToKeyValueData()).ToListAsync();
        }
    }
}
