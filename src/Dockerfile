# 1. Estágio de Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copia o projeto e restaura as dependências
COPY ["FCG.CatalogAPI.csproj", "./"]
RUN dotnet restore "FCG.CatalogAPI.csproj"

# Copia o restante do código
COPY . .

# Publica a aplicação
RUN dotnet publish "FCG.CatalogAPI.csproj" \
    -c Release \
    -o /app/publish \
    /p:UseAppHost=false

# 2. Estágio de Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "FCG.CatalogAPI.dll"]