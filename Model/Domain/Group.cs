using EntityFrameworkCore.Projectables;

namespace Model.Domain
{
    public class Group : Activable, Auditable
    {
        public long Id { get; set; }

        public bool IsActive { get; set; } = true;
        public string? CreationUser { get; set; }
        public DateTime? CreationDate { get; set; }
        public string? LastUpdateUser { get; set; }
        public DateTime? LastUpdateDate { get; set; }

        public string Code { get; set; }
        public string Name { get; set; }

        public virtual List<Product> Products { get; set; }

        [Projectable]
        public string CodeAndName => $"{Code} - {Name}";
    }
}
