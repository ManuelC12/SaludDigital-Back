# USAMOS IMAGENES DE LA VERSION 6
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

COPY . .

RUN dotnet restore
RUN dotnet publish -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:80
EXPOSE 80

# RECUERDA: Cambia "SaludDigital.dll" si tu proyecto se llama diferente
ENTRYPOINT ["dotnet", "SaludDigital.dll"]