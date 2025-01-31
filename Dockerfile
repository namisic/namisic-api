FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY src/Condominiums.Api/Condominiums.Api.csproj ./Condominiums.Api/Condominiums.Api.csproj
RUN dotnet restore ./Condominiums.Api/Condominiums.Api.csproj
COPY src .
RUN dotnet publish ./Condominiums.Api/Condominiums.Api.csproj -c Release -o /publish

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /publish
COPY --from=build /publish .
EXPOSE 80
ENTRYPOINT ["dotnet", "Condominiums.Api.dll"]
