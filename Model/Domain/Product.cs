using EntityFrameworkCore.Projectables;
using Microsoft.EntityFrameworkCore;
using Model.Utils;

namespace Model.Domain
{
    public class Product : Activable, Auditable
    {
        public long Id { get; set; }

        public bool IsActive { get; set; } = true;
        public string? CreationUser { get; set; }
        public DateTime? CreationDate { get; set; }
        public string? LastUpdateUser { get; set; }
        public DateTime? LastUpdateDate { get; set; }

        public string Code { get; set;}
        public string Name { get;set; }
        public string? Description { get; set; }
        public long? CategoryId { get; set; }
        public decimal SalePrice { get; set; }
        public decimal IvaPercentage { get; set; }

        public virtual Category Category { get; set; }

        public virtual List<ProductImage> ProductImages { get; set; } = new List<ProductImage>();
        public virtual List<ProductOffer> ProductOffers { get; set; } = new List<ProductOffer>();

        [Projectable]
        public string CodeAndNameAndDescription => $"{Code} - {Name} - {SalePrice:$#,##0.00}";

        [Projectable]
        public decimal PriceByQuantity(int quantity) => !ProductOffers.Any(po => po.IsActive && po.Units == quantity) ? SalePrice : ProductOffers.First(po => po.IsActive && po.Units == quantity).UnitPrice;
    }
}
