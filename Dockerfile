FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Japaninja/Japaninja.WebApp.csproj", "Japaninja/"]
RUN dotnet restore "Japaninja/Japaninja.WebApp.csproj"
COPY . .
WORKDIR "/src/Japaninja"
RUN dotnet build "Japaninja.WebApp.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Japaninja.WebApp.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Japaninja.WebApp.dll"]