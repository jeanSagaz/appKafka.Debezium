What is the appKafka.Debezium Project?
=====================
The appKafka.Debezium Project is a open-source project written in .NET Core

The goal of this project is implement the most common used technologies and share with the technical community the best way to develop great applications with .NET

## Give a Star! :star:
If you liked the project or if appKafka.Debezium helped you, please give a star ;)

## How to use:
- You will need the latest Visual Studio 2022 and the latest .NET Core SDK.
- The latest SDK and tools can be downloaded from https://dot.net/core.

Also you can run the appKafka.Debezium Project in Visual Studio Code (Windows, Linux or MacOS).

To know more about how to setup your enviroment visit the [Microsoft .NET Download Guide](https://www.microsoft.com/net/download)

- Install [Docker](https://docs.docker.com/docker-for-windows/install/).

## Technologies implemented:

- ASP.NET Core 7.0 (with .NET Core 7.0)
 - ASP.NET Worker Service
- .NET Core Native DI
- Kafka
- Debezium

## Debezium:

- [Debezium configuration SqlServer version 2.3](https://debezium.io/documentation/reference/2.3/index.html)
As the documentation recommends.<br/>
Warning: Do not change the value of this property. If you change the name value, after a restart, instead of continuing to emit events to the original topics, the connector emits subsequent events to topics whose names are based on the new value. The connector is also unable to recover its database schema history topic.

- [Debezium configuration SqlServer version 1.1](https://debezium.io/documentation/reference/1.1/connectors/sqlserver.html)

## Running the project
Go to the folder where the file is located `docker-compose.yaml` and at the command prompt run `docker-compose up -d`.
If you are using the connector `debezium-connector-sqlserver:latest` use the postman collection `connector-configure-latest`.
If you are using the connector `debezium-connector-sqlserver:1.1.0` use the postman collection `connector-configure`.

# Documentation for consultation
* [Debezium](https://debezium.io/)