# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy solution and project
COPY OTSupply.sln ./
COPY OTSupply.API/OTSupply.API.csproj OTSupply.API/

# Restore
RUN dotnet restore

# Copy everything else
COPY . .

# Publish
RUN dotnet publish OTSupply.API/OTSupply.API.csproj -c Release -o /app/publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

COPY --from=build /app/publish .

EXPOSE 8080

ENV ASPNETCORE_URLS=http://+:8080

ENTRYPOINT ["dotnet", "OTSupply.API.dll"]
