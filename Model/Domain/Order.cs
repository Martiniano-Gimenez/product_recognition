using EntityFrameworkCore.Projectables;
using Microsoft.EntityFrameworkCore;

namespace Model.Domain
{
    public class Order : Auditable
    {
        public long Id { get; set; }

        public string? CreationUser { get; set; }
        public DateTime? CreationDate { get; set; }
        public string? LastUpdateUser { get; set; }
        public DateTime? LastUpdateDate { get; set; }

        public DateTime Date { get; set; } = DateTime.Now;
        public string? Observation { get; set; }
        [Precision(18, 4)]
        public decimal Net { get; set; }
        [Precision(18, 4)]
        public decimal Iva { get; set; }
        [Precision(18, 4)]
        public decimal Total { get; set; }
        public eOrderState OrderState { get; set; } = eOrderState.Analysis;

        public long ClientId { get; set; }
        public long? SellerId { get; set; }

        public virtual Client Client { get; set; }  
        public virtual Seller Seller { get; set; }  

        public virtual List<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
        public virtual List<OrderHistory> OrderHistories { get; set; } = new List<OrderHistory>();

        [Projectable]
        public string SellerDisplay => SellerId.HasValue ? Seller.Name : "Sin vendedor";
    }
}
