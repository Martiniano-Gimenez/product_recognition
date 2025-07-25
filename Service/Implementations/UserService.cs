using Microsoft.EntityFrameworkCore;
using Model.Domain;
using Model.Repositories;
using Service.Data;
using Service.Mappers;
using Service.ServiceContracts;

namespace Service.Implementations
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<UserData?> Login(LoginData data)
        {
            return await _unitOfWork.UserRepository.Login(data.UserName, CryptographyService.Encrypt(data.Password))
                .Select(UserMappingExtensions.MapToData()).FirstOrDefaultAsync();
        }

        public async Task<bool> ChangePassword(ChangePasswordData data)
        {
            var user = await _unitOfWork.UserRepository.ChangePassword(data.Id, CryptographyService.Encrypt(data.CurrentPassword)).FirstOrDefaultAsync()
                ?? throw new BusinessException("La contraseña ingresada no coincide con la anterior");

            user.Password = CryptographyService.Encrypt(data.NewPassword);
            user.HasToChangePassword = false;

            return await _unitOfWork.SaveChangesAsync();
        }

        public async Task Create(string userName, string password, long? clientId, long? sellerId)
        {
            if(await _unitOfWork.UserRepository.ExistsAnyWithUserName(userName))
                throw new BusinessException($"Ya existe un usuario con el nombre {userName}");

            if (string.IsNullOrWhiteSpace(userName) || string.IsNullOrWhiteSpace(password))
                throw new BusinessException("Debe completar usuario y contraseña");

            if (!clientId.HasValue && !sellerId.HasValue)
                throw new BusinessException("Ocurrio un error al crear el usuario");

            await _unitOfWork.UserRepository.CreateAsync(new User 
            { 
                UserName = userName,
                Password = CryptographyService.Encrypt(password),
                RoleId = clientId.HasValue ? eRole.Purchasing : eRole.StockManager,
                ClientId = clientId,
                SellerId = sellerId,
                HasToChangePassword = true
            });

            await _unitOfWork.SaveChangesAsync();
        }

        public async Task Edit(string oldUserName, string newUserName)
        {
            if (await _unitOfWork.UserRepository.ExistsAnyWithUserName(newUserName))
                throw new BusinessException($"Ya existe un usuario con el nombre {newUserName}");

            if (string.IsNullOrWhiteSpace(oldUserName) || string.IsNullOrWhiteSpace(newUserName))
                throw new BusinessException("Ocurrio un error al modificar el usuario");

            var user = await _unitOfWork.UserRepository.GetByCondition(u => u.UserName == oldUserName).FirstOrDefaultAsync()
                ?? throw new BusinessException("No se encontró usuario con el nombre anterior");
            user.UserName = oldUserName;

            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<GridData<UserGridData>> GetAllPaginated(DTParameters param)
        {
            var users = _unitOfWork.UserRepository
                .GetFilteredByPage(param.SearchValue, param.OrderBy, param.SortDirection(DTOrderDir.DESC))
                .Select(UserMappingExtensions.MapForGridData());

            var usersPaginated = await _unitOfWork.UserRepository.GetPagedListAsync(users, param.Start, param.Length);
            return usersPaginated.MapToPagGridData();
        }

        public async Task<bool> ResetPassword(long userId)
        {
            var user = await _unitOfWork.UserRepository.GetByCondition(u => u.Id == userId).FirstOrDefaultAsync();

            if (user is null)
                return false;

            user.Password = CryptographyService.Encrypt(user.UserName);
            user.HasToChangePassword = true;
            return await _unitOfWork.SaveChangesAsync();
        }
    }
}
