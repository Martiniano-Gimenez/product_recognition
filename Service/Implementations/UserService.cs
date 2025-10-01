using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Model.Domain;
using Model.Repositories;
using Model.Utils;
using Service.Data;
using Service.Mappers;
using Service.ServiceContracts;
using System.ComponentModel;

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

        public async Task<UserData> GetById(long id)
        {
            return await _unitOfWork.UserRepository.GetByCondition(c => c.Id == id)
                .Select(UserMappingExtensions.MapToData()).FirstOrDefaultAsync() ?? throw new BusinessException("El usuario no existe");
        }

        public async Task<bool> Create(UserData data)
        {
            if (await _unitOfWork.UserRepository.ExistsAnyWithUserName(data.UserName))
                throw new BusinessException("Ya existe ese nombre de usuario");

            var user = data.MapToEntity();
            user.Password = CryptographyService.Encrypt(user.UserName);
            user.HasToChangePassword = true;
            await _unitOfWork.UserRepository.CreateAsync(user);

            return await _unitOfWork.SaveChangesAsync();
        }

        public async Task<bool> Edit(UserData data)
        {
            if (await _unitOfWork.UserRepository.ExistsAnyWithUserName(data.UserName, data.UserId))
                throw new BusinessException("Ya existe ese nombre de usuario");

            var entity = await _unitOfWork.UserRepository.GetByCondition(c => c.Id == data.UserId).FirstOrDefaultAsync()
                ?? throw new BusinessException("El usuario no existe");

            entity = data.MapToEntity(entity);

            return await _unitOfWork.SaveChangesAsync();
        }

        public async Task<bool> Delete(long id)
        {
            if (!await _unitOfWork.UserRepository.ExistsOtherAdmin(id))
                throw new BusinessException("No puede eliminar este ususario porque no hay otro administrador");

            var entity = await _unitOfWork.UserRepository.GetByCondition(sf => sf.Id == id).FirstOrDefaultAsync();

            if (entity is null)
                return false;

            _unitOfWork.UserRepository.Remove(entity);

            return await _unitOfWork.SaveChangesAsync();
        }

        public List<KeyValueData> GetAllRoles()
        {
            return Enum.GetValues(typeof(eRole)).Cast<eRole>().Select(s => new KeyValueData
            {
                Key = Convert.ToInt32(s),
                Value = s.GetAttribute<DescriptionAttribute>().Description
            }).ToList();
        }
    }
}
