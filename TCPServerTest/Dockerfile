FROM mcr.microsoft.com/dotnet/runtime:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["TCPServerTest/TCPServerTest.csproj", "TCPServerTest/"]
RUN dotnet restore "TCPServerTest/TCPServerTest.csproj"
COPY . .
WORKDIR "/src/TCPServerTest"
RUN dotnet build "TCPServerTest.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TCPServerTest.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TCPServerTest.dll"]
