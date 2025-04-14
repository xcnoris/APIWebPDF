# Etapa 1: Imagem base com o runtime do .NET para produção
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
# Define as variáveis de ambiente para produção
ENV ASPNETCORE_ENVIRONMENT=Production
ENV ASPNETCORE_URLS=http://0.0.0.0:5007
EXPOSE 5007

# Cria a pasta Boletos no container
RUN mkdir -p /app/Boletos

# Etapa 2: Imagem com o SDK do .NET para build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copia todos os arquivos da sua API para dentro do container
COPY . .

# Publica o projeto para uma pasta otimizada
RUN dotnet publish -c Release -o /app/publish

# Etapa final: Imagem de runtime para produção
FROM base AS final
WORKDIR /app

# Copia os arquivos publicados na etapa de build para a imagem final
COPY --from=build /app/publish .

# Inicia a aplicação
ENTRYPOINT ["dotnet", "APIWebPDF.dll"]
