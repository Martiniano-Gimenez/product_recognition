using EntityFrameworkCore.Projectables;
using Microsoft.EntityFrameworkCore;

namespace Model.Domain
{
    public class ProductOffer : Activable, Auditable
    {
        public long Id { get; set; }

        public bool IsActive { get; set; } = true;
        public string? CreationUser { get; set; }
        public DateTime? CreationDate { get; set; }
        public string? LastUpdateUser { get; set; }
        public DateTime? LastUpdateDate { get; set; }

        [Precision(18, 4)]
        public decimal UnitPrice { get; set; }
        public int Units { get; set; }  

        public long ProductId { get; set; }

        public virtual Product Product { get; set; }

    }
}
