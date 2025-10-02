using Model.Domain;
using Model.Utils;
using Service.Data;
using System.Linq.Expressions;

namespace Service.Mappers
{
    public static class DepositMovementMappingExtensions
    {
        public static Expression<Func<DepositMovement, DepositMovement>> MapForGridData()
        {
            return entity => new DepositMovement
            {
                Id = entity.Id,
                Date = entity.Date,
                OriginDeposit = entity.OriginDepositId.HasValue ? new Deposit { Identifier = entity.OriginDeposit.Identifier } : null,
                DestinationDeposit = entity.DestinationDepositId.HasValue ? new Deposit { Identifier = entity.DestinationDeposit.Identifier } : null,
            };
        }

        public static GridData<DepositMovementGridData> MapToPagGridData(this PagingResult<DepositMovement> entity)
        {
            return new GridData<DepositMovementGridData>
            {
                Count = entity.Count,
                List = entity.Results.Select(x => x.MapToGridData()).ToList()
            };
        }

        public static DepositMovementGridData MapToGridData(this DepositMovement entity)
        {
            return new DepositMovementGridData
            {
                Id = entity.Id,
                Date = entity.Date.ToString("dd/MM/yyyy HH:mm"),
                OriginDeposit = entity.OriginDeposit?.Identifier,
                DestinationDeposit = entity.DestinationDeposit?.Identifier,
            };
        }

        public static Expression<Func<DepositMovement, DepositMovementData>> MapToDepositMovementData()
        {
            return entity => new DepositMovementData
            {
                Id = entity.Id,
                OriginDepositId = entity.OriginDepositId,
                DestinationDepositId = entity.DestinationDepositId,
                Date = entity.Date.ToString("dd/MM/yyyy HH:mm"),
                Observation = entity.Observation,
                Products = entity.DepositMovementDetails.AsQueryable().Where(od => od.IsActive)
                    .Select(DepositMovementDetailMappingExtensions.MapToDepositMovementDetailData()).ToList(),
            };
        }

        public static Expression<Func<DepositMovement, DepositMovementViewData>> MapToDepositMovementViewData()
        {
            return entity => new DepositMovementViewData
            {
                Id = entity.Id,
                OriginDeposit = entity.OriginDeposit.Identifier,
                DestinationDeposit = entity.DestinationDeposit.Identifier,
                Date = entity.Date.ToString("dd/MM/yyyy HH:mm"),
                Observation = entity.Observation,
                Products = entity.DepositMovementDetails.AsQueryable().Where(od => od.IsActive)
                    .Select(DepositMovementDetailMappingExtensions.MapToDepositMovementDetailViewData()).ToList(),
            };
        }

        public static DepositMovement MapToEntity(this DepositMovementData data)
        {
            return new DepositMovement
            {
                OriginDepositId = data.OriginDepositId,
                DestinationDepositId = data.DestinationDepositId,
                Observation = data.Observation,
                DepositMovementDetails = data.Products.Select(p => new DepositMovementDetail
                {
                    ProductId = p.ProductId,
                    Quantity = p.Quantity,
                }).ToList(),
            };
        }
    }
}
