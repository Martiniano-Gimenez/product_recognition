namespace Model.Domain
{
    public class OrderHistory
    {
        public long Id { get; set; }

        public eOrderState OrderState { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;

        public long OrderId { get; set; }
        public long UserId {  get; set; }

        public virtual Order Order { get; set; }
        public virtual User User { get; set; }
    }
}
