namespace Model.Domain
{
    public class ProductImage : Activable, Auditable
    {
        public long Id { get; set; }

        public bool IsActive { get; set; } = true;
        public string? CreationUser { get; set; }
        public DateTime? CreationDate { get; set; }
        public string? LastUpdateUser { get; set; }
        public DateTime? LastUpdateDate { get; set; }

        public string? Path { get; set; }

        public long ProductId { get; set; }

        public virtual Product Product { get; set; }

    }
}
