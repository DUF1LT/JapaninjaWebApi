FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443
EXPOSE 2472

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Japaninja.Api/Japaninja.Api.csproj", "Japaninja.Api/"]
RUN dotnet restore "Japaninja.Api/Japaninja.Api.csproj"
COPY . .
WORKDIR "/src/Japaninja.Api"
RUN dotnet build "Japaninja.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Japaninja.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Japaninja.Api.dll"]