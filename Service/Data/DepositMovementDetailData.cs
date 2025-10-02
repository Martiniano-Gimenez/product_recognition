namespace Service.Data
{
    public class DepositMovementDetailData
    {
        public long? Id { get; set; }
        public long ProductId { get; set; }
        public string Code { get; set; } 
        public string Name { get; set; }
        public int Quantity { get; set; }
    }
}
