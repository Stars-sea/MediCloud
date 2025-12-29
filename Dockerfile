FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /MediCloud

COPY . ./

RUN dotnet restore ./MediCloud.Api/MediCloud.Api.csproj
RUN dotnet publish ./MediCloud.Api/MediCloud.Api.csproj -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /MediCloud

COPY --from=build /MediCloud/out .

ENTRYPOINT ["dotnet", "MediCloud.Api.dll"]
