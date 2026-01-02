FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /app

COPY . ./

RUN dotnet restore ./MediCloud.Api/MediCloud.Api.csproj
RUN dotnet publish ./MediCloud.Api/MediCloud.Api.csproj \
    -c Release \
    -o out \
    --self-contained false \
    -p:PublishReadyToRun=true \
    -p:PublishSingleFile=false

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:10.0-alpine
WORKDIR /app

COPY --from=build /app/out .

ENTRYPOINT ["dotnet", "MediCloud.Api.dll"]
