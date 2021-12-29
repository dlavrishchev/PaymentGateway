using System;
using Grpc.Core;
using PaymentGateway.Billing.Domain;

namespace PaymentGateway.Billing.Grpc;

internal static class MetadataExtensions
{
    public static Metadata WithErrorCode(this Metadata metadata, string errorCode)
    {
        Guard.NotNullOrWhiteSpace(errorCode, nameof(errorCode));

        const string ENTRY_NAME = "error_code";
        var entry = metadata.Get(ENTRY_NAME);
        if (entry != null)
            throw new InvalidOperationException("Unable to add 'error_code' metadata entry. Entry already exists.");

        metadata.Add(ENTRY_NAME, errorCode);
        return metadata;
    }
}