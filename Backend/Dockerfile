FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["PublicServiceRequestBackend/PublicServiceRequestBackend.csproj", "PublicServiceRequestBackend/"]
RUN dotnet restore "PublicServiceRequestBackend/PublicServiceRequestBackend.csproj"
COPY . .
WORKDIR "/src/PublicServiceRequestBackend"
RUN dotnet build "PublicServiceRequestBackend.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "PublicServiceRequestBackend.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PublicServiceRequestBackend.dll"]