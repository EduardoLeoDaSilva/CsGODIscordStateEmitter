FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
RUN apt-get update && apt-get install -y libx11-6 libx11-xcb1 libatk1.0-0 libgtk-3-0 libcups2 libdrm2 libxkbcommon0 libxcomposite1 libxdamage1 libxrandr2 libgbm1 libpango-1.0-0 libcairo2 libasound2 libxshmfence1 libnss3
WORKDIR /app
EXPOSE 80
EXPOSE 443

ENV ASPNETCORE_URLS=http://+:5218

# Creates a non-root user with an explicit UID and adds permission to access the /app folder
# For more info, please refer to https://aka.ms/vscode-docker-dotnet-configure-containers
RUN adduser -u 5678 --disabled-password --gecos "" appuser && chown -R appuser /app
USER appuser

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["CsGOStateEmitter.csproj", "./"]
RUN dotnet restore "CsGOStateEmitter.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "CsGOStateEmitter.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CsGOStateEmitter.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CsGOStateEmitter.dll"]
