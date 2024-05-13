FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["PlayerWallet.Api/PlayerWallet.Api.csproj", "PlayerWallet.Api/"]
COPY ["PlayerWallet.Model/PlayerWallet.Model.csproj", "PlayerWallet.Model/"]
COPY ["PlayerWallet.Repository/PlayerWallet.Repository.csproj", "PlayerWallet.Repository/"]
RUN dotnet restore "PlayerWallet.Api/PlayerWallet.Api.csproj"
COPY . .
WORKDIR "/src/PlayerWallet.Api"
RUN dotnet build "PlayerWallet.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "PlayerWallet.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PlayerWallet.Api.dll"]
