using Service.Data;

namespace Service.ServiceContracts
{
    public interface IProductDetectorService
    {
        public List<ProductDetectionResult> DetectProductsFromStream(Stream imageStream);
    }
}
