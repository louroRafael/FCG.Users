# Base image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

# Build image
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copia os arquivos .csproj e restaura
COPY ["FCG.Users.API/FCG.Users.API.csproj", "FCG.Users.API/"]
COPY ["FCG.Users.Domain/FCG.Users.Domain.csproj", "FCG.Users.Domain/"]
COPY ["FCG.Users.Infra/FCG.Users.Infra.csproj", "FCG.Users.Infra/"]
COPY ["FCG.Users.Services/FCG.Users.Services.csproj", "FCG.Users.Services/"]
RUN dotnet restore "FCG.Users.API/FCG.Users.API.csproj"

# Copia o código e build
COPY . .
WORKDIR "/src/FCG.Users.API"
RUN dotnet build "FCG.Users.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publish
FROM build AS publish
RUN dotnet publish "FCG.Users.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Runtime final
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV ASPNETCORE_URLS=http://+:8080
ENTRYPOINT ["dotnet", "FCG.Users.API.dll"]