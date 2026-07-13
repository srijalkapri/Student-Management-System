# Build context is the solution root (CRUD/)
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY CRUD.Domain/CRUD.Domain.csproj CRUD.Domain/
COPY CRUD.Application/CRUD.Application.csproj CRUD.Application/
COPY CRUD.Infrastructure/CRUD.Infrastructure.csproj CRUD.Infrastructure/
COPY CRUD.Web/CRUD.Web.csproj CRUD.Web/

RUN dotnet restore CRUD.Web/CRUD.Web.csproj

COPY CRUD.Domain/ CRUD.Domain/
COPY CRUD.Application/ CRUD.Application/
COPY CRUD.Infrastructure/ CRUD.Infrastructure/
COPY CRUD.Web/ CRUD.Web/

RUN dotnet publish CRUD.Web/CRUD.Web.csproj -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# Render provides PORT; default to 8080 for local docker runs
ENV ASPNETCORE_ENVIRONMENT=Production
ENV ASPNETCORE_URLS=http://0.0.0.0:8080

COPY --from=build /app/publish .

EXPOSE 8080

# Prefer Render's $PORT when present
CMD ["sh", "-c", "dotnet CRUD.dll --urls http://0.0.0.0:${PORT:-8080}"]
