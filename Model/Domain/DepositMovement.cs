using EntityFrameworkCore.Projectables;
using Microsoft.EntityFrameworkCore;

namespace Model.Domain
{
    public class DepositMovement : Auditable
    {
        public long Id { get; set; }

        public string? CreationUser { get; set; }
        public DateTime? CreationDate { get; set; }
        public string? LastUpdateUser { get; set; }
        public DateTime? LastUpdateDate { get; set; }

        public DateTime Date { get; set; } = DateTime.Now;
        public string? Observation { get; set; }

        public long? OriginDepositId { get; set; }
        public long? DestinationDepositId { get; set; }

        public virtual Deposit OriginDeposit { get; set; }  
        public virtual Deposit DestinationDeposit { get; set; }  

        public virtual List<DepositMovementDetail> DepositMovementDetails { get; set; } = new List<DepositMovementDetail>();
    }
}
