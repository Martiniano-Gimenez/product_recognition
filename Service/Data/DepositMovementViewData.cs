using Model.Utils;
using System.ComponentModel.DataAnnotations;

namespace Service.Data
{
    public class DepositMovementViewData
    {
        public long Id { get; set; }

        [Display(Name = "Depósito orígen")]
        public string OriginDeposit { get; set; }

        [Display(Name = "Depósito destino")]
        public string DestinationDeposit { get; set; }

        [Display(Name = "Fecha")]
        public string Date { get; set; }

        [Display(Name = "Total")]
        public decimal Total { get; set; }

        [Display(Name = "Observación")]
        public string? Observation { get; set; }


        public List<DepositMovementDetailViewData> Products { get; set; } = new List<DepositMovementDetailViewData>();

        public string DisplayTotal => Total.AsMoneyString();
    }
}
