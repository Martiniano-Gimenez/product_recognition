using Microsoft.EntityFrameworkCore;
using Model.Repositories;
using Service.Data;
using Service.ServiceContracts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using System.Linq;

public class ProductDetectorService : IProductDetectorService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly string _productsPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "ProductImages");

    public ProductDetectorService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
        Directory.CreateDirectory(_productsPath);
    }

    public List<ProductDetectionResult> DetectProductsFromStream(Stream imageStream)
    {
        using var photo = Image.Load<Rgba32>(imageStream);
        photo.Mutate(x => x.Resize(256, 256).Grayscale());

        var detections = new List<ProductDetectionResult>();
        var productFiles = Directory.GetFiles(_productsPath);

        foreach (var productFile in productFiles)
        {
            if (!int.TryParse(Path.GetFileNameWithoutExtension(productFile), out int productImageId))
                continue;

            var productData = _unitOfWork.ProductImageRepository.GetAll()
                .Include(pi => pi.Product)
                .Where(pi => pi.Id == productImageId)
                .Select(pi => new
                {
                    pi.Id,
                    pi.ProductId,
                    ProductName = pi.Product.Name
                })
                .FirstOrDefault();

            if (productData == null)
                continue;

            using var productImage = Image.Load<Rgba32>(productFile);
            productImage.Mutate(x => x.Resize(256, 256).Grayscale());

            ulong productHash = ComputeImageHash(productImage);
            ulong photoHash = ComputeImageHash(photo);
            double histDiff = Math.Abs(ComputeHistogramScore(productImage) - ComputeHistogramScore(photo));
            double edgeDiff = Math.Abs(ComputeEdgeScore(productImage) - ComputeEdgeScore(photo));

            var productColor = GetDominantColorFiltered(productImage);
            var photoColor = GetDominantColorFiltered(photo);

            double colorDiff = Math.Sqrt(
                Math.Pow(productColor.R - photoColor.R, 2) +
                Math.Pow(productColor.G - photoColor.G, 2) +
                Math.Pow(productColor.B - photoColor.B, 2)
            );

            int diffHash = HammingDistance(productHash, photoHash);
            double scoreTotal = diffHash * 2.0 + histDiff * 50 + edgeDiff * 0.5;

            bool isRedDominant = photoColor.R > photoColor.G + 15 && photoColor.R > photoColor.B + 15;
            bool isMatch = false;

            if (scoreTotal < 30 && colorDiff < 180 && isRedDominant)
                isMatch = true;
            else if (scoreTotal < 45 && colorDiff < 120 && isRedDominant)
                isMatch = true;
            else if (scoreTotal < 35 && colorDiff < 80)
                isMatch = true;

            if (!isRedDominant && colorDiff > 70)
                isMatch = false;

            if (isMatch && !detections.Any(d => d.ProductId == productData.ProductId))
            {
                detections.Add(new ProductDetectionResult
                {
                    ProductImageId = productData.Id,
                    ProductId = productData.ProductId,
                    ProductName = productData.ProductName,
                    Difference = scoreTotal
                });

                Console.WriteLine($"✅ {productData.ProductName} detectado (score={scoreTotal:F2}, colorDiff={colorDiff:F2}, R={photoColor.R}, G={photoColor.G}, B={photoColor.B})");
            }
            else
            {
                Console.WriteLine($"🔍 {productData.ProductName} descartado (score={scoreTotal:F2}, colorDiff={colorDiff:F2}, R={photoColor.R}, G={photoColor.G}, B={photoColor.B})");
            }
        }

        return detections.OrderBy(d => d.Difference).ToList();
    }

    private static ulong ComputeImageHash(Image<Rgba32> image)
    {
        using var resized = image.Clone(x => x.Resize(16, 16).Grayscale());
        double avg = 0;
        var pixels = new byte[256];
        int i = 0;

        for (int y = 0; y < 16; y++)
        {
            for (int x = 0; x < 16; x++)
            {
                var p = resized[x, y];
                pixels[i++] = p.R;
                avg += p.R;
            }
        }

        avg /= 256.0;
        ulong hash = 0;
        for (int j = 0; j < 64; j++)
        {
            if (pixels[j] >= avg)
                hash |= 1UL << j;
        }

        return hash;
    }

    private static double ComputeHistogramScore(Image<Rgba32> image)
    {
        var bins = new double[16];
        using var small = image.Clone(x => x.Resize(64, 64).Grayscale());

        for (int y = 0; y < small.Height; y++)
        {
            for (int x = 0; x < small.Width; x++)
            {
                var val = small[x, y].R / 16;
                bins[val]++;
            }
        }

        double total = bins.Sum();
        for (int i = 0; i < bins.Length; i++)
            bins[i] /= total;

        return bins.Sum(b => b * b);
    }

    private static double ComputeEdgeScore(Image<Rgba32> image)
    {
        using var gray = image.Clone(x => x.Resize(64, 64).Grayscale());
        double totalDiff = 0;
        int count = 0;

        for (int y = 1; y < gray.Height; y++)
        {
            for (int x = 1; x < gray.Width; x++)
            {
                var p = gray[x, y];
                var px = gray[x - 1, y];
                var py = gray[x, y - 1];

                totalDiff += Math.Abs(p.R - px.R);
                totalDiff += Math.Abs(p.R - py.R);
                count += 2;
            }
        }

        return totalDiff / count;
    }

    private static Rgba32 GetDominantColorFiltered(Image<Rgba32> image)
    {
        long r = 0, g = 0, b = 0;
        int count = 0;

        for (int y = 0; y < image.Height; y += 2)
        {
            for (int x = 0; x < image.Width; x += 2)
            {
                var pixel = image[x, y];
                int brightness = (pixel.R + pixel.G + pixel.B) / 3;

                if (brightness > 230 || brightness < 20)
                    continue;

                r += pixel.R;
                g += pixel.G;
                b += pixel.B;
                count++;
            }
        }

        if (count == 0) count = 1;

        return new Rgba32(
            (byte)(r / count),
            (byte)(g / count),
            (byte)(b / count)
        );
    }

    private static int HammingDistance(ulong h1, ulong h2)
    {
        ulong x = h1 ^ h2;
        int count = 0;
        while (x != 0)
        {
            count += (int)(x & 1);
            x >>= 1;
        }
        return count;
    }
}
