FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

COPY "Services/Checkout/Checkout.Api/Checkout.Api.csproj" "Services/Checkout/Checkout.Api/Checkout.Api.csproj"
COPY "NuGet.config" "NuGet.config"

RUN dotnet restore "Services/Checkout/Checkout.Api/Checkout.Api.csproj"

COPY . .
RUN dotnet publish "Services/Checkout/Checkout.Api" -c Release -o /out --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY --from=build /out .

ENTRYPOINT ["dotnet", "Checkout.Api.dll"]