FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["EmailWorker/EmailWorker.csproj", "EmailWorker/"]
RUN dotnet restore "EmailWorker/EmailWorker.csproj"
COPY . .
RUN dotnet publish "EmailWorker/EmailWorker.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "EmailWorker.dll"]