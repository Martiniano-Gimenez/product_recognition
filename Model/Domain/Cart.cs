namespace Model.Domain
{
    public class Cart : Activable, Auditable
    {
        public long Id { get; set; }

        public bool IsActive { get; set; } = true;
        public string? CreationUser { get; set; }
        public DateTime? CreationDate { get; set; }
        public string? LastUpdateUser { get; set; }
        public DateTime? LastUpdateDate { get; set; }


        public long UserId { get; set; }

        public virtual User Client { get; set; }

        public virtual List<CartDetail> CartDetails { get; set; } = new();
    }
}
