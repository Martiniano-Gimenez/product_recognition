using Microsoft.EntityFrameworkCore;

namespace Model.Domain
{
    public class OrderDetail : Activable, Auditable
    {
        public long Id { get; set; }

        public bool IsActive { get; set; } = true;
        public string? CreationUser { get; set; }
        public DateTime? CreationDate { get; set; }
        public string? LastUpdateUser { get; set; }
        public DateTime? LastUpdateDate { get; set; }

        public decimal UnitPrice { get; set; }
        public decimal Net { get; set; }
        public decimal IvaPercentage { get; set; }
        public decimal Total { get; set; }
        public int Quantity { get; set; }

        public long ProductId { get; set; }
        public long OrderId { get; set; }

        public virtual Product Product { get; set; }
        public virtual Order Order { get; set; }
    }
}
