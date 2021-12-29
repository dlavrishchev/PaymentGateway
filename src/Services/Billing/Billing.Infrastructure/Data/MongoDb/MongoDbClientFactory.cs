using MongoDB.Driver;

namespace Billing.Infrastructure.Data.MongoDb;

public static class MongoDbClientFactory
{
    public static MongoClient Create(MongoDbOptions options)
    {
        var mongoUrl = new MongoUrl(options.ConnectionString);
        var settings = MongoClientSettings.FromUrl(mongoUrl);
        // if (options.LogEnabled)
        // {
        //     settings.ClusterConfigurator = clusterBuilder =>
        //     {
        //         clusterBuilder.Subscribe<CommandStartedEvent>(e =>
        //         {
        //             logger.LogDebug("MongoDb command started: {CommandName} - {CommandToJson}", e.CommandName, e.Command.ToJson());
        //         });
        //     };
        // }
        return new MongoClient(settings);
    }
}