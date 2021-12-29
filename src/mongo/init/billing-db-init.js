function getMerchants() {
    return [
        {
            "_id": ObjectId("6036347d1617680f6fb192ce"),
            "Company": {
                "Name": "merchant1",
            },
            "Status": 0,
            "CreatedAt": new Date()
        }
    ];
}

function getPaymentMethods() {
    return [
        {
            "_id": ObjectId("6036347d1617680f6fb192cc"),
            "Name": "Bank card",
            "MinPaymentAmount": NumberDecimal(10),
            "MaxPaymentAmount": NumberDecimal(100000),
            "Currency": "RUB",
            "IsActive": true,
            "Type": 0
        }
    ];
}

function getShops() {
    return [
        {
            "_id": ObjectId("6036347d1617680f6fb192d2"),
            "MerchantId": ObjectId("6036347d1617680f6fb192ce"),
            "Name": "Shop1",
            "Description": "shop1 description",
            "Url": "http://shop1.example.com",
            "WebhookNotification":
            {
                "NotificationUrl": "http://shop1.example.com/wh",
                "SecretKey": "1F_g5MN"
            },
            "RedirectDetails":
            {
                "FailUrl": "http://shop1.example.com/fail",
                "SuccessUrl": "http://shop1.example.com/success"
            },
            "CreatedAt": new Date()
        }
    ];
}

function getCurrencies() {
    return [
        {
            "_id": ObjectId("6037ec73eec711a37b22ce03"),
            "Code": "RUB",
            "Name": "Russian ruble"
        },
        {
            "_id": ObjectId("6037ec88eec711a37b22ce04"),
            "Code": "USD",
            "Name": "US dollar"
        }
    ];
}

function initMerchantCollection() {
    let merchants = getMerchants();
    db.Merchant.insertMany(merchants);
    print('Merchant collection successfully initialized.');
}

function initPaymentMethodCollection() {
    let paymentMethods = getPaymentMethods();
    db.PaymentMethod.insertMany(paymentMethods);
    print('PaymentMethod collection successfully initialized.');
}

function initShopCollection() {
    let shops = getShops();
    db.Shop.insertMany(shops);
    print('Shop collection successfully initialized.');
}

function initCurrencyCollection() {
    let currencies = getCurrencies();
    db.Currency.insertMany(currencies);
    db.Currency.createIndex( { Code: 1 } )
    print('Currency collection successfully initialized.');
}

function initInvoiceCollection() {
    db.Invoice.createIndex( { Guid: 1 } )
    print('Invoice collection successfully initialized.');
}

function init() {
    const dbName = 'billing';
    print(`Starting initialize '${dbName}' database...`);
    db = db.getSiblingDB(dbName);

    initMerchantCollection();
    initPaymentMethodCollection();
    initShopCollection();
    initCurrencyCollection();
    initInvoiceCollection();

    print(`Database '${dbName}' has been successfully initialized.`);
}

init();
