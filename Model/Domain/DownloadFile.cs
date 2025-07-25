namespace Model.Domain
{
    public class DownloadFile : Activable, Auditable
    {
        public long Id { get; set; }

        public bool IsActive { get; set; } = true;
        public string? CreationUser { get; set; }
        public DateTime? CreationDate { get; set; }
        public string? LastUpdateUser { get; set; }
        public DateTime? LastUpdateDate { get; set; }

        public string Name { get; set; }
        public int Order { get; set; }
        public eFileExtension FileExtension { get; set; }
    }
}
