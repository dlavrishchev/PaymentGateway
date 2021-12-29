namespace Billing.Infrastructure.Data.MongoDb;

public class MongoDbOptions
{
    public const string SectionName = "MongoDb";

    public string ConnectionString { get; set; }
    public string Database { get; set; }
    public bool LogEnabled { get; set; }
}