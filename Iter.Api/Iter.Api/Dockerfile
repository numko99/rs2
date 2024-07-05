FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app


FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY . .

FROM build AS publish
RUN dotnet publish "Iter.Api/Iter.Api.csproj" -c Release -o /app
FROM base AS final
WORKDIR /app
COPY --from=publish /app .


ARG RABBITMQ_HOSTNAME
ARG RABBITMQ_PORT
ARG RABBITMQ_USERNAME
ARG RABBITMQ_PASSWORD

ENV RabbitMqSettings__HostName=$RABBITMQ_HOSTNAME
ENV RabbitMqSettings__Port=$RABBITMQ_PORT
ENV RabbitMqSettings__Username=$RABBITMQ_USERNAME
ENV RabbitMqSettings__Password=$RABBITMQ_PASSWORD

ENTRYPOINT ["dotnet", "Iter.Api.dll"]