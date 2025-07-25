using EntityFrameworkCore.Projectables;

namespace Model.Domain
{
    public class User : Activable, Auditable
    {
        public long Id { get; set; }

        public bool IsActive { get; set; } = true;
        public string? CreationUser { get; set; }
        public DateTime? CreationDate { get; set; }
        public string? LastUpdateUser { get; set; }
        public DateTime? LastUpdateDate { get; set; }

        public string? UserName { get; set; }
        public string? Password { get; set; }
        public eRole RoleId { get; set; }
        public bool HasToChangePassword { get; set; } = true;

        public long? ClientId { get; set; }
        public long? SellerId { get;set; }

        public virtual Client Client { get; set; }
        public virtual Seller Seller { get; set; }

        [Projectable]
        public string GridDataName => SellerId.HasValue ? Seller.CuilWithName : (ClientId.HasValue ? Client.CuilWithName : string.Empty);
    }
}
