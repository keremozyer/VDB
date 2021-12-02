#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["VDB.MicroServices.InventoryManager.Web.API/VDB.MicroServices.InventoryManager.Web.API.csproj", "VDB.MicroServices.InventoryManager.Web.API/"]
RUN dotnet restore "VDB.MicroServices.InventoryManager.Web.API/VDB.MicroServices.InventoryManager.Web.API.csproj"
COPY . .
WORKDIR "/src/VDB.MicroServices.InventoryManager.Web.API"
RUN dotnet build "VDB.MicroServices.InventoryManager.Web.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "VDB.MicroServices.InventoryManager.Web.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "VDB.MicroServices.InventoryManager.Web.API.dll"]