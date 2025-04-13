# Etapa 1: imagem base com o runtime do .NET (roda a aplicação)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

# Cria a pasta Boletos no container
RUN mkdir -p /app/Boletos

# Etapa 2: imagem com o SDK do .NET para buildar o projeto
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copia todos os arquivos da sua API para dentro do container
COPY . .

# Publica o projeto para uma pasta otimizada
RUN dotnet publish -c Release -o /app/publish

# Etapa final: roda o app com runtime puro
FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .

# Inicia o app
ENTRYPOINT ["dotnet", "APIWebPDF.dll"]
