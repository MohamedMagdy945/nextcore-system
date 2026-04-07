using Catalog.Core.Entities;
using MongoDB.Driver;
using System.Text.Json;

namespace Catalog.Infrastructure.Data.Seed
{
    public class TypeSeeder
    {
        public static async Task SeedAsync(IMongoCollection<ProductType> typeCollection)
        {
            var hasData = await typeCollection.Find(_ => true).AnyAsync();
            if (hasData) return;

            var filePath = Path.Combine("Data", "Seed", "Files", "types.json");

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
