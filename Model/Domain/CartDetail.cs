namespace Model.Domain
{
    public class CartDetail : Activable, Auditable
    {
        public long Id { get; set; }

        public bool IsActive { get; set; } = true;
        public string? CreationUser { get; set; }
        public DateTime? CreationDate { get; set; }
        public string? LastUpdateUser { get; set; }
        public DateTime? LastUpdateDate { get; set; }

        public int Quantity { get; set; }

        public long ProductId { get; set; }
        public long CartId { get; set; }

        public virtual Product Product { get; set; }
        public virtual Cart Cart { get; set; }  

    }
}
