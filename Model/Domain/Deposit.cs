
using EntityFrameworkCore.Projectables;

namespace Model.Domain
{
    public class Deposit : Activable, Auditable
    {
        public long Id { get; set; }

        public bool IsActive { get; set; } = true;
        public string? CreationUser { get; set; }
        public DateTime? CreationDate { get; set; }
        public string? LastUpdateUser { get; set; }
        public DateTime? LastUpdateDate { get; set; }

        public string? Identifier { get; set; }
        public string Description { get; set; }

        public virtual List<User> Users { get; set; } = new();
    }
}
