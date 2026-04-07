namespace Catalog.Infrastructure.Common.Settings;

public class DatabaseSettings
{
    public string ConnectionString { get; set; } = null!;
    public string DatabaseName { get; set; } = null!;
    public string Brands { get; set; } = null!;
    public string Types { get; set; } = null!;
    public string Products { get; set; } = null!;
}
