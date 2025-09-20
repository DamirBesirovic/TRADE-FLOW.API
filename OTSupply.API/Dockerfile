# Step 1: Build the .NET application using the .NET SDK image
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# Set the working directory inside the container
WORKDIR /OTSupply.API

# Copy the .csproj file and restore any dependencies (via NuGet)
COPY ["OTSupply.API.csproj", "."]

# Restore the dependencies
RUN dotnet restore

# Copy the rest of the application code
COPY . .

# Build the application
RUN dotnet publish "OTSupply.API.csproj" -c Release -o /app/publish

# Step 2: Create the runtime image using the .NET runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime

# Set the working directory inside the container
WORKDIR /app

# Copy the built application from the build stage
COPY --from=build /app/publish .

# Expose the port the app will listen on (default is 80 for ASP.NET Core)
EXPOSE 8080

# Define the entry point for the application
ENTRYPOINT ["dotnet", "OTSupply.API.dll"]
