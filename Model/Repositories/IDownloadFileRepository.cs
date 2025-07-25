using Model.Domain;

namespace Model.Repositories
{
    public interface IDownloadFileRepository : IRepository<DownloadFile, long>
    {
        IQueryable<DownloadFile> GetFilteredByPage(string filter, string orderBy, string sortDirection, short fileExtensionId);
    }
}
