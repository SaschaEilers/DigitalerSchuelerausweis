FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Schuelerausweis/Schuelerausweis.csproj", "Schuelerausweis/"]
RUN dotnet restore "Schuelerausweis/Schuelerausweis.csproj"
COPY . .
WORKDIR "/src/Schuelerausweis"
RUN dotnet build "Schuelerausweis.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Schuelerausweis.csproj" -c Release -o /app/publish

FROM base AS final
RUN apt-get update \
    && apt-get install -y --no-install-recommends \
        libldap-2.4-2 \
    && rm -rf /var/lib/apt/lists/*
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Schuelerausweis.dll"]
