# Etapa de construcción
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Restaurar paquetes
COPY ["ApiUsersRecipeTool.csproj", "ApiUsersRecipeTool/"]
RUN dotnet restore "ApiUsersRecipeTool/ApiUsersRecipeTool.csproj"

# Copiar y construir la aplicación
COPY . .
WORKDIR "/src/ApiUsersRecipeTool"
RUN dotnet build "ApiUsersRecipeTool.csproj" -c Release -o /app/build

# Etapa de publicación
FROM build AS publish
RUN dotnet publish "ApiUsersRecipeTool.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Etapa final
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ApiUsersRecipeTool.dll"]