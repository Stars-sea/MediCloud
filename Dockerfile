FROM mcr.microsoft.com/dotnet/sdk:10.0-alpine AS build
WORKDIR /app

COPY . ./

# Use Alpine mirrors for faster package downloads in China
RUN echo -e "https://mirrors.tuna.tsinghua.edu.cn/alpine/v$(cat /etc/alpine-release | cut -d'.' -f1-2)/main/\nhttps://mirrors.tuna.tsinghua.edu.cn/alpine/v$(cat /etc/alpine-release | cut -d'.' -f1-2)/community/" > /etc/apk/repositories

RUN apk add --no-cache \
    protobuf-dev

RUN dotnet restore ./MediCloud.Api/MediCloud.Api.csproj \
    --use-current-runtime \
    --runtime linux-musl-x64
RUN dotnet publish ./MediCloud.Api/MediCloud.Api.csproj -c Release -o out \
    --use-current-runtime \
    --runtime linux-musl-x64

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:10.0-alpine
WORKDIR /app

COPY --from=build /app/out .

ENTRYPOINT ["dotnet", "MediCloud.Api.dll"]
