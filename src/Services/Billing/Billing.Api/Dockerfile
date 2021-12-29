FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

COPY "Services/Billing/Billing.Api/Billing.Api.csproj" "Services/Billing/Billing.Api/Billing.Api.csproj"
COPY "Services/Billing/Billing.Domain/Billing.Domain.csproj" "Services/Billing/Billing.Domain/Billing.Domain.csproj"
COPY "Services/Billing/Billing.Infrastructure/Billing.Infrastructure.csproj" "Services/Billing/Billing.Infrastructure/Billing.Infrastructure.csproj"
COPY "Services/Billing/Billing.UnitTests/Billing.UnitTests.csproj" "Services/Billing/Billing.UnitTests/Billing.UnitTests.csproj"
COPY "NuGet.config" "NuGet.config"

RUN dotnet restore "Services/Billing/Billing.Api/Billing.Api.csproj"

COPY . .
RUN dotnet publish "Services/Billing/Billing.Api/Billing.Api.csproj" -c Release -o /out --no-restore

FROM build as unittest
WORKDIR "Services/Billing/Billing.UnitTests"

FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY --from=build /out .

ENTRYPOINT ["dotnet", "Billing.Api.dll"]