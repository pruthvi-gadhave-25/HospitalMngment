# Stage 1: Build the app
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj and restore dependencies
COPY ["HospitalManagement.csproj", "./"]
RUN dotnet restore

# Copy the rest of the app and publish
COPY . .
RUN dotnet publish "HospitalManagement.csproj" -c Release -o /app/publish

# Stage 2: Create final image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# Copy published output from build stage
COPY --from=build /app/publish .


EXPOSE 5000

# Set environment variable to bind to port 5000
ENV ASPNETCORE_URLS=http://+:5000

# Entry point
ENTRYPOINT ["dotnet", "HospitalManagement.dll"]
