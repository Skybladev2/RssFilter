#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0.1-alpine3.12-amd64 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0.101-alpine3.12-amd64 AS build
WORKDIR /src
COPY ["RssFilter/RssFilter.csproj", "RssFilter/"]
RUN sed -i '/<Target /,/<\/Target>/d' RssFilter/RssFilter.csproj
RUN cat RssFilter/RssFilter.csproj
RUN dotnet restore "RssFilter/RssFilter.csproj"
COPY . .
WORKDIR "/src/RssFilter"
RUN sed -i '/<Target /,/<\/Target>/d' RssFilter.csproj
RUN cat RssFilter.csproj
RUN dotnet build "RssFilter.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "RssFilter.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RssFilter.dll"]