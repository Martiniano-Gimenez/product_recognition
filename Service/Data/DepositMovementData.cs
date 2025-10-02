using Model.Utils;
using System.ComponentModel.DataAnnotations;

namespace Service.Data
{
    public class DepositMovementData
    {
        public long Id { get; set; }

        [Display(Name = "Depósito orígen")]
        public long? OriginDepositId { get; set; }

        [Display(Name = "Depósito destino")]
        public long? DestinationDepositId { get; set; }

        [Display(Name = "Fecha")]
        public string Date { get; set; } = DateTime.Now.ToString("dd/MM/yyyy HH:mm");

        [Display(Name = "Observación")]
        public string? Observation { get; set; }

        [Display(Name = "Agregar artículo")]
        public long? NewProductId { get; set; }

        [Display(Name = "Cantidad")]
        public int? NewProductQuantity { get; set; }

        public List<DepositMovementDetailData> Products { get; set; } = new List<DepositMovementDetailData>();
    }
}
