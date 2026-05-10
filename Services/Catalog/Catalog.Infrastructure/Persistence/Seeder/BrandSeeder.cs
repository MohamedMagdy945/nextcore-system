using Catalog.Core.Entities;
using MongoDB.Driver;
using System.Text.Json;

namespace Catalog.Infrastructure.Persistence.Seeder
{
    public class BrandSeeder
    {
        public static async Task SeedAsync(IMongoCollection<ProductBrand> brandCollection)
        {
            var hasData = await brandCollection.Find(FilterDefinition<ProductBrand>.Empty).AnyAsync();
            if (hasData) return;

            var filePath = Path.Combine(AppContext.BaseDirectory, "Persistence", "Files", "brands.json");

            if (!File.Exists(filePath))
            {

                Console.WriteLine($"Seed file {filePath} was not found.");
                return;
            }
            var brandJsonData = await File.ReadAllTextAsync(filePath);
            var brands = JsonSerializer.Deserialize<List<ProductBrand>>(brandJsonData);

            if (brands?.Any() is true)
            {
                await brandCollection.InsertManyAsync(brands);
            }
        }
    }
}
