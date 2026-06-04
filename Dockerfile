FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY MediaBizzare/MediaBizzare.csproj MediaBizzare/
RUN dotnet restore MediaBizzare/MediaBizzare.csproj

COPY MediaBizzare/ MediaBizzare/
RUN dotnet publish MediaBizzare/MediaBizzare.csproj -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

ENV ASPNETCORE_HTTP_PORTS=8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "MediaBizzare.dll"]
