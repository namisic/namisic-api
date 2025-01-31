# API de NamiSIC

Este repositorio contiene la API de la aplicación NamiSIC, construida con .NET 9 usando el lenguaje C#.

# Ejecución local con Docker Compose

Primero debes tener instalado Docker Compose en la máquina. Sigue [la guía de instalación](https://docs.docker.com/compose/install/) si no lo tienes.

## Contenedor de MongoDB

Ejecuta el siguiente comando para crear el contenedor de base de datos:

```
docker compose -f ./docker-compose-local.yml up -d mongodb
```

Crea la base de datos y el usuario ejecutando el script desde este comando:

```
docker compose -f ./docker-compose-local.yml exec mongodb mongosh --host 127.0.0.1 --port 27017 --username eladmin --password namisic_2025 --file /data/db/scripts/create-database.js
```

## Contenedor de la API

Para crear el contenedos de la API, ejecuta el siguiente comando:

```
docker compose -f ./docker-compose-local.yml up -d api
```

Visita http://localhost:5000/swagger para consultar la definición de la API.

# Más información

Si deseas conocer más del proyecto visita mi blog: https://lechediaz.com/category/proyectos/namisic/
