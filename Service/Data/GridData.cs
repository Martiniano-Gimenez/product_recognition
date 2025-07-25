namespace Service.Data
{
    public class GridData<T>
    {
        public long Count { get; set; }
        public List<T> List { get; set; } = new List<T>();
    }
}
