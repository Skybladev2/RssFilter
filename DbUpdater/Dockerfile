#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src
COPY ["DbUpdater/DbUpdater.csproj", "DbUpdater/"]
RUN sed -i '/<Target /,/<\/Target>/d' DbUpdater/DbUpdater.csproj
RUN cat DbUpdater/DbUpdater.csproj
COPY ["RssFilter.Models/RssFilter.Models.csproj", "RssFilter.Models/"]
RUN dotnet restore "DbUpdater/DbUpdater.csproj"
COPY . .
WORKDIR "/src/DbUpdater"
RUN sed -i '/<Target /,/<\/Target>/d' DbUpdater.csproj
RUN cat DbUpdater.csproj
RUN dotnet build "DbUpdater.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DbUpdater.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DbUpdater.dll"]