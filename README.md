# API de Nami SIC

Este repositorio contiene la API de la aplicación Nami SIC, construida con .NET 9 usando el lenguaje C#. Pensada para ser usada junto a una base de datos MongoDB y alguna solución IAM como SimpleIdServer o IdentityServer.

# Ejecución local con Docker Compose

Primero debes tener instalado Docker Compose en la máquina. Sigue [la guía de instalación](https://docs.docker.com/compose/install/) si no lo tienes.

## Crear red de Docker

Se debe crear una red de Docker para permitir la comunicación entre los contenedores:

```
docker network create -d bridge --attachable namisic_local_net
```

## Crear base de datos

Primero se debe crear el contenedor de MongoDB:

```
docker compose -f ./docker-compose.local.yml up -d mongodb
```

Una vez creado el contenedor, se debe ejecutar el script [create_local_database.js](./mongodb/create_local_database.js) que es el que se encarga de crear la base de datos:

```
docker compose -f ./docker-compose.local.yml exec mongodb mongosh --file /data/db/scripts/create_local_database.js
```

## Crear contenedor de la API

Finalmente, crea el contenedor la API:

```
docker compose -f ./docker-compose.local.yml up -d api
```

> Visita http://localhost:5000/swagger para consultar la definición de la API.

# Más información

Si deseas conocer más del proyecto [visita mi blog](https://lechediaz.com/category/proyectos/namisic/).
