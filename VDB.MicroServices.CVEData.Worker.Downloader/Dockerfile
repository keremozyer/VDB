#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:5.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["VDB.MicroServices.CVEData.Worker.Downloader/VDB.MicroServices.CVEData.Worker.Downloader.csproj", "VDB.MicroServices.CVEData.Worker.Downloader/"]
RUN dotnet restore "VDB.MicroServices.CVEData.Worker.Downloader/VDB.MicroServices.CVEData.Worker.Downloader.csproj"
COPY . .
WORKDIR "/src/VDB.MicroServices.CVEData.Worker.Downloader"
RUN dotnet build "VDB.MicroServices.CVEData.Worker.Downloader.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "VDB.MicroServices.CVEData.Worker.Downloader.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "VDB.MicroServices.CVEData.Worker.Downloader.dll"]