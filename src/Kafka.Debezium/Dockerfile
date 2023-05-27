#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/Kafka-Debezium/Kafka-Debezium.csproj", "src/Kafka-Debezium/"]
RUN dotnet restore "src/Kafka-Debezium/Kafka-Debezium.csproj"
COPY . .
WORKDIR "/src/src/Kafka-Debezium"
RUN dotnet build "Kafka-Debezium.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Kafka-Debezium.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Kafka-Debezium.dll"]