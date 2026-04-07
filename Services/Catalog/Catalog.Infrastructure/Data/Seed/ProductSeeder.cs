using Catalog.Core.Entities;
using MongoDB.Driver;
using System.Text.Json;

namespace Catalog.Infrastructure.Data.Seed
{
    public class CatalogSeeder
    {
        public static async Task SeedDatasync(IMongoCollection<Product> productCollection)
        {
            var hasData = await productCollection.Find(_ => true).AnyAsync();
            if (hasData) return;

            var filePath = Path.Combine("Data", "Seed", "Files", "products.json");

            if (!File.Exists(filePath))
            {

                Console.WriteLine($"Seed file {filePath} was not found.");
                return;
            }
            var productJsonData = await File.ReadAllTextAsync(filePath);
            var products = JsonSerializer.Deserialize<List<Product>>(productJsonData);

            if (products?.Any() is true)
            {
                await productCollection.InsertManyAsync(products);
            }
        }
    }
}
