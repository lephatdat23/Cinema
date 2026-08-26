FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["Cinema.Pricing.gRPC/Cinema.Pricing.gRPC.csproj", "Cinema.Pricing.gRPC/"]
RUN dotnet restore "Cinema.Pricing.gRPC/Cinema.Pricing.gRPC.csproj"
COPY . .
WORKDIR "/src/Cinema.Pricing.gRPC"
RUN dotnet build "Cinema.Pricing.gRPC.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Cinema.Pricing.gRPC.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Cinema.Pricing.gRPC.dll"]