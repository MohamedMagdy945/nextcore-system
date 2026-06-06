using Catalog.Core.Entities;
using MongoDB.Driver;
using System.Text.Json;

namespace Catalog.Infrastructure.Persistence.Seeder
{
    public class CategorySeeder
    {
        public static async Task SeedAsync(IMongoCollection<Category> typeCollection)
        {
            var hasData = await typeCollection.Find(FilterDefinition<Category>.Empty).AnyAsync();
            if (hasData) return;

            var filePath = Path.Combine(AppContext.BaseDirectory, "Persistence", "Files", "categories.json");

            if (!File.Exists(filePath))
            {

                Console.WriteLine($"Seed file {filePath} was not found.");
                return;
            }
            var typeJsonData = await File.ReadAllTextAsync(filePath);
            var types = JsonSerializer.Deserialize<List<Category>>(typeJsonData);

            if (types?.Any() is true)
            {
                await typeCollection.InsertManyAsync(types);
            }
        }
    }
}
