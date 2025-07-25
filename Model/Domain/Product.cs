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

        [Projectable]
        public string CodeAndNameAndDescription => $"{Code} - {Name} - {ShortDescription}";

        [Projectable]
        public string ShortDescription => Description.Length > 50 ? Description.Substring(0, 50) : Description;
    }
}
