FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["Cinema.Booking.API/Cinema.Booking.API.csproj", "Cinema.Booking.API/"]
COPY ["Cinema.Pricing.gRPC/Cinema.Pricing.gRPC.csproj", "Cinema.Pricing.gRPC/"]
RUN dotnet restore "Cinema.Booking.API/Cinema.Booking.API.csproj"
COPY . .
WORKDIR "/src/Cinema.Booking.API"
RUN dotnet build "Cinema.Booking.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Cinema.Booking.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Cinema.Booking.API.dll"]