# --- ETAPA 1: BUILD ---
# Usamos el SDK de .NET 9.0 para poder entender tus paquetes nuevos
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY . .

# Restauramos y compilamos
RUN dotnet restore
RUN dotnet publish -c Release -o /app/publish

# --- ETAPA 2: RUN ---
# Usamos el Runtime de .NET 9.0 para ejecutar
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build /app/publish .

# Render necesita esto
ENV ASPNETCORE_URLS=http://+:80
EXPOSE 80

# IMPORTANTE: Revisa que el nombre del DLL sea el correcto
ENTRYPOINT ["dotnet", "SaludDigital.dll"]