using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Bson.Serialization.Serializers;
using PaymentGateway.Billing.Domain.Entities.Currencies;
using PaymentGateway.Billing.Domain.Entities.Invoices;
using PaymentGateway.Billing.Domain.Entities.Merchants;
using PaymentGateway.Billing.Domain.Entities.Payments;
using PaymentGateway.Billing.Domain.Entities.Shops;

namespace Billing.Infrastructure.Data.MongoDb;

internal static class MongoDbRegistrar
{
    public static void Register()
    {
        RegisterSerializers();
        RegisterConventions();
        RegisterClassMaps();
    }

    private static void RegisterSerializers()
    {
        BsonSerializer.RegisterSerializer(new DecimalSerializer(BsonType.Decimal128));
    }
        
    private static void RegisterConventions()
    {
        var cp = new ConventionPack
        {
            new StringIdStoredAsObjectIdConvention()
        };
        ConventionRegistry.Register("AppConventions", cp, filter:t => true);
    }
        
    private static void RegisterClassMaps()
    {
        BsonClassMap.RegisterClassMap<Shop>(cm =>
        {
            cm.AutoMap();
            cm.GetMemberMap(c => c.MerchantId).SetSerializer(new StringSerializer(BsonType.ObjectId));
            cm.SetIdMember(cm.GetMemberMap(c => c.Id));
        });

        BsonClassMap.RegisterClassMap<Invoice>(cm =>
        {
            cm.AutoMap();
            cm.GetMemberMap(c => c.ShopId).SetSerializer(new StringSerializer(BsonType.ObjectId));
            cm.SetIdMember(cm.GetMemberMap(c => c.Id));
        });

        BsonClassMap.RegisterClassMap<Merchant>(cm =>
        {
            cm.AutoMap();
            cm.SetIdMember(cm.GetMemberMap(c => c.Id));
        });

        BsonClassMap.RegisterClassMap<PaymentMethod>(cm =>
        {
            cm.AutoMap();
            cm.SetIdMember(cm.GetMemberMap(c => c.Id));
        });

        BsonClassMap.RegisterClassMap<Currency>(cm =>
        {
            cm.AutoMap();
            cm.SetIdMember(cm.GetMemberMap(c => c.Id));
        });
    }
}