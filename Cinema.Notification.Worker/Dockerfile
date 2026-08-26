FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["Cinema.Notification.Worker/Cinema.Notification.Worker.csproj", "Cinema.Notification.Worker/"]
RUN dotnet restore "Cinema.Notification.Worker/Cinema.Notification.Worker.csproj"
COPY . .
WORKDIR "/src/Cinema.Notification.Worker"
RUN dotnet build "Cinema.Notification.Worker.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Cinema.Notification.Worker.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Cinema.Notification.Worker.dll"]