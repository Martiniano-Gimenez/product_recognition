namespace Model.Domain
{
    public class User : Activable, Auditable
    {
        public long Id { get; set; }

        public bool IsActive { get; set; } = true;
        public string? CreationUser { get; set; }
        public DateTime? CreationDate { get; set; }
        public string? LastUpdateUser { get; set; }
        public DateTime? LastUpdateDate { get; set; }

        public string? UserName { get; set; }
        public string? Password { get; set; }
        public eRole RoleId { get; set; }
        public bool HasToChangePassword { get; set; } = true;
    }
}
