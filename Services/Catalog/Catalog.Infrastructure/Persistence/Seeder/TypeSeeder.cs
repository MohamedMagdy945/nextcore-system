using Catalog.Core.Entities;
using MongoDB.Driver;
using System.Text.Json;

namespace Catalog.Infrastructure.Persistence.Seeder
{
    public class TypeSeeder
    {
        public static async Task SeedAsync(IMongoCollection<ProductType> typeCollection)
        {
            var hasData = await typeCollection.Find(FilterDefinition<ProductType>.Empty).AnyAsync();
            if (hasData) return;

            var filePath = Path.Combine(AppContext.BaseDirectory, "Persistence", "Files", "brands.json");

            if (!File.Exists(filePath))
            {

                Console.WriteLine($"Seed file {filePath} was not found.");
                return;
            }
            var typeJsonData = await File.ReadAllTextAsync(filePath);
            var types = JsonSerializer.Deserialize<List<ProductType>>(typeJsonData);

            if (types?.Any() is true)
            {
                await typeCollection.InsertManyAsync(types);
            }
        }
    }
}
