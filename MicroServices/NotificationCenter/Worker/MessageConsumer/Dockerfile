#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:5.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["VDB.MicroServices.NotificationCenter.Worker.MessageConsumer/VDB.MicroServices.NotificationCenter.Worker.MessageConsumer.csproj", "VDB.MicroServices.NotificationCenter.Worker.MessageConsumer/"]
RUN dotnet restore "VDB.MicroServices.NotificationCenter.Worker.MessageConsumer/VDB.MicroServices.NotificationCenter.Worker.MessageConsumer.csproj"
COPY . .
WORKDIR "/src/VDB.MicroServices.NotificationCenter.Worker.MessageConsumer"
RUN dotnet build "VDB.MicroServices.NotificationCenter.Worker.MessageConsumer.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "VDB.MicroServices.NotificationCenter.Worker.MessageConsumer.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "VDB.MicroServices.NotificationCenter.Worker.MessageConsumer.dll"]