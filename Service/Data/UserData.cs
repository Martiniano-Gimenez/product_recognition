namespace Service.Data
{
    public class UserData
    {
        public string UserName { get; set; }
        public int RoleId { get; set; }
        public string DisplayName { get; set; }
        public long UserId { get; set; }
        public long? SellerId { get; set; }
        public bool HasToChangePassword { get; set; }
    }
}
