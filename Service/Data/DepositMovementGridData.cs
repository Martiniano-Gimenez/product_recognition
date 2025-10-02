namespace Service.Data
{
    public class DepositMovementGridData : BaseGridData
    {
        public long Id { get; set; }
        public string Date { get; set; }    
        public string? OriginDeposit { get; set; }
        public string? DestinationDeposit { get; set; }
    }
}
