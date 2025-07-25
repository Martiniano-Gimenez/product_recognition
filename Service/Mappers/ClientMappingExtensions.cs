using Model.Domain;
using Model.Utils;
using Service.Data;
using System.Linq.Expressions;

namespace Service.Mappers
{
    public static class ClientMappingExtensions
    {
        public static Expression<Func<Client, Client>> MapForGridData()
        {
            return entity => new Client
            {
                Id = entity.Id,
                Name = entity.Name,
                CUIL = entity.CUIL,
                Email = entity.Email,
                CellPhone = entity.CellPhone,
                Telephone = entity.Telephone,
            };
        }

        public static GridData<ClientGridData> MapToPagGridData(this PagingResult<Client> entity)
        {
            return new GridData<ClientGridData>
            {
                Count = entity.Count,
                List = entity.Results.Select(x => x.MapToGridData()).ToList()
            };
        }

        public static ClientGridData MapToGridData(this Client entity)
        {
            return new ClientGridData
            {
                Id = entity.Id,
                Name = entity.Name,
                CUIL = entity.CUIL,
                Email = entity.Email,
                CellPhone = entity.CellPhone?.ToString() ?? string.Empty,
                Telephone = entity.Telephone?.ToString() ?? string.Empty,
            };
        }

        public static Expression<Func<Client, KeyValueData>> MapToKeyValueData()
        {
            return entity => new KeyValueData
            {
                Key = entity.Id,
                Value = entity.Name
            };
        }

        public static Expression<Func<Client, ClientData>> MapToData()
        {
            return entity => new ClientData
            {
                Id = entity.Id,
                Name = entity.Name,
                CUIL = entity.CUIL,
                Email = entity.Email,
                CellPhone = entity.CellPhone,
                Telephone = entity.Telephone,
            };
        }

        public static Client MapToEntity(this ClientData data)
        {
            return new Client
            {
                Name = data.Name,
                CUIL = data.CUIL,
                Email = data.Email,
                CellPhone = data.CellPhone,
                Telephone = data.Telephone,
            };
        }

        public static Client MapToEntity(this ClientData data, Client entity)
        {
            entity.Name = data.Name;
            entity.CUIL = data.CUIL;
            entity.Email = data.Email;
            entity.CellPhone = data.CellPhone;
            entity.Telephone = data.Telephone;
            return entity;

        }

        public static Expression<Func<Client, ClientViewData>> MapToViewData()
        {
            return entity => new ClientViewData
            {
                Name = entity.Name,
                CUIL = entity.CUIL,
                Email = entity.Email,
                CellPhone = entity.CellPhone,
                Telephone = entity.Telephone,
            };
        }
    }
}
