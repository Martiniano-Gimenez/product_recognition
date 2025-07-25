namespace Model.Domain
{
    public class PagingResult<T>
    {
        public long Count { get; set; }
        public List<T> Results { get; set; } = new();
    }
}
