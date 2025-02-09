# API de NamiSIC

Este repositorio contiene la API de la aplicación NamiSIC, construida con .NET 9 usando el lenguaje C#.

# Ejecución local con Docker Compose

Primero debes tener instalado Docker Compose en la máquina. Sigue [la guía de instalación](https://docs.docker.com/compose/install/) si no lo tienes.

## Crear red de Docker

Para permitir la comunicación entre los contenedores se debe crear una red de Docker con el siguiente comando:

```
docker network create -d bridge --attachable namisic_local_net
```

## Crear contenedores

Para crear los contenedores de MongoDB y la API, ejecuta el siguiente comando:

```
docker compose -f ./docker-compose-local.yml up -d
```

Espera a que termine la ejecución del comando y visita http://localhost:5000/swagger para consultar la definición de la API.

# Más información

Si deseas conocer más del proyecto visita mi blog: https://lechediaz.com/category/proyectos/namisic/
