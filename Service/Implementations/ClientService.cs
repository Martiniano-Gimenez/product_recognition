using Microsoft.EntityFrameworkCore;
using Model.Repositories;
using Service.Data;
using Service.Mappers;
using Service.ServiceContracts;

namespace Service.Implementations
{
    public class ClientService : IClientService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserService _userService;

        public ClientService(IUnitOfWork unitOfWork,
                             IUserService userService) 
        {
            _unitOfWork = unitOfWork;
            _userService = userService;
        }

        public async Task<GridData<ClientGridData>> GetAllPaginated(DTParameters param)
        {
            var clients = _unitOfWork.ClientRepository
                .GetFilteredByPage(param.SearchValue, param.OrderBy, param.SortDirection(DTOrderDir.DESC))
                .Select(ClientMappingExtensions.MapForGridData());

            var clientsPaginated = await _unitOfWork.ClientRepository.GetPagedListAsync(clients, param.Start, param.Length);
            return clientsPaginated.MapToPagGridData();
        }

        public async Task<bool> Create(ClientData data)
        {
            if (await _unitOfWork.ClientRepository.ExistsAnyWithCuil(data.CUIL))
                throw new BusinessException($"Ya existe un cliente con el CUIL {data.CUIL}");

            await _unitOfWork.BeginTransactionAsync();

            var newClient = data.MapToEntity();
            await _unitOfWork.ClientRepository.CreateAsync(newClient);

            if (!await _unitOfWork.SaveChangesAsync())
                return false;

            await _userService.Create(newClient.CUIL, newClient.CUIL, newClient.Id, null);

            await _unitOfWork.CommitAsync();
            return true ;
        }

        public async Task<ClientData> GetById(long id)
        {
            return await _unitOfWork.ClientRepository.GetByCondition(c => c.Id == id)
                .Select(ClientMappingExtensions.MapToData()).FirstOrDefaultAsync() ?? throw new BusinessException("El cliente no existe");
        }

        public async Task<bool> Edit(ClientData data)
        {
            var client = await _unitOfWork.ClientRepository.GetByCondition(c => c.Id == data.Id).FirstOrDefaultAsync()
                ?? throw new BusinessException("El cliente no existe");

            if (await _unitOfWork.ClientRepository.ExistsAnyWithCuil(data.CUIL, client.Id))
                throw new BusinessException($"Ya existe un cliente con el CUIL {data.CUIL}");

            await _unitOfWork.BeginTransactionAsync();
            var oldCuil = client.CUIL;

            data.MapToEntity(client);

            if (!await _unitOfWork.SaveChangesAsync())
                return false;

            if(oldCuil != client.CUIL)
                await _userService.Edit(oldCuil, client.CUIL);

            await _unitOfWork.CommitAsync();
            return true;
        }

        public async Task<ClientViewData> GetViewById(long id)
        {
            return await _unitOfWork.ClientRepository.GetByCondition(c => c.Id == id)
                .Select(ClientMappingExtensions.MapToViewData()).FirstOrDefaultAsync() ?? throw new BusinessException("El cliente no existe");
        }

        public async Task<List<KeyValueData>> GetAllSelectable()
        {
            return await _unitOfWork.ClientRepository.GetAll()
                .Select(ClientMappingExtensions.MapToKeyValueData()).ToListAsync();
        }

        public async Task<List<KeyValueData>> GetAllSelectableBySellerId(long sellerId)
        {
            return await _unitOfWork.ClientRepository.GetBySellerId(sellerId)
                .Select(ClientMappingExtensions.MapToKeyValueData()).ToListAsync();
        }
    }
}
