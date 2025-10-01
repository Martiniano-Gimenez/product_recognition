using Model.Domain;
using Model.Utils;
using Service.Data;
using System.ComponentModel;
using System.Linq.Expressions;

namespace Service.Mappers
{
    public static class UserMappingExtensions
    {
        public static Expression<Func<User, UserData>> MapToData()
        {
            return entity => new UserData
            {
                UserId = entity.Id, 
                UserName = entity.UserName,
                RoleId = (int)entity.RoleId,
                HasToChangePassword = entity.HasToChangePassword
            };
        }

        public static Expression<Func<User, User>> MapWithRoleAndIds()
        {
            return entity => new User
            {
                Id = entity.Id,
                RoleId = entity.RoleId,
                SellerId =  entity.SellerId,
            };
        }

        public static Expression<Func<User, User>> MapForGridData()
        {
            return entity => new User
            {
                Id = entity.Id,
                UserName = entity.UserName,
                RoleId = entity.RoleId,
            };
        }

        public static GridData<UserGridData> MapToPagGridData(this PagingResult<User> entity)
        {
            return new GridData<UserGridData>
            {
                Count = entity.Count,
                List = entity.Results.Select(x => x.MapToGridData()).ToList()
            };
        }

        public static UserGridData MapToGridData(this User entity)
        {
            return new UserGridData
            {
                Id = entity.Id,
                UserName = entity.UserName,
                Role = entity.RoleId.GetAttribute<DescriptionAttribute>().Description,
                Name = entity.GridDataName
            };
        }

        public static User MapToEntity(this UserData data)
        {
            return new User
            {
                UserName = data.UserName,
                RoleId = (eRole)data.RoleId
            };
        }

        public static User MapToEntity(this UserData data, User entity)
        {
            entity.UserName = data.UserName;
            entity.RoleId = (eRole)data.RoleId;
            return entity;

        }
    }
}
