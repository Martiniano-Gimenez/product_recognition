
using EntityFrameworkCore.Projectables;

namespace Model.Domain
{
    public class Client : Activable, Auditable
    {
        public long Id { get; set; }

        public bool IsActive { get; set; } = true;
        public string? CreationUser { get; set; }
        public DateTime? CreationDate { get; set; }
        public string? LastUpdateUser { get; set; }
        public DateTime? LastUpdateDate { get; set; }

        public string? Name { get; set; }
        public long? CellPhone { get; set; }
        public long? Telephone { get; set; }
        public string? Email { get; set; }
        public string? CUIL { get; set; }
        public string? Address { get; set; }

        public virtual List<User> Users { get; set; } = new();

        [Projectable]
        public string CuilWithName => $"{CUIL} - {Name}";
    }
}
