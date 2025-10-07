namespace Service.Data
{
    public class ProductDetectionResult
    {
        public long ProductImageId { get; set; }
        public long ProductId { get; set; }
        public string ProductName { get; set; } = "";
        public int X { get; set; }
        public int Y { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public double Difference { get; set; }
    }
}
