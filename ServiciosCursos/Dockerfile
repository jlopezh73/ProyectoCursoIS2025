# Utiliza la imagen oficial de .NET para aplicaciones ASP.NET Core
FROM mcr.microsoft.com/dotnet/aspnet:latest AS base
WORKDIR /app
EXPOSE 80

# Imagen para construir la aplicación
FROM mcr.microsoft.com/dotnet/sdk:latest AS build
WORKDIR /src

# Copia los archivos de proyecto y restaura dependencias
COPY . .
RUN dotnet restore

# Publica la aplicación en modo Release
RUN dotnet publish -c Release -o /app/publish

# Imagen final
FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "ServiciosCursos.dll"]