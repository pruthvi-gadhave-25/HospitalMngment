FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["HospitalManagement.csproj", "./"]
RUN dotnet restore "HospitalManagement.csproj"
COPY . .
RUN dotnet publish "HospitalManagement.csproj" -c Release -o /app/publish


FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 80
ENTRYPOINT ["dotnet", "HospitalManagement.dll"]