FROM mcr.microsoft.com/dotnet/sdk:6.0-alpine as build
WORKDIR /source
COPY . .
RUN dotnet restore "./eec-backend/eec-backend.csproj" --disable-parallel
RUN dotnet publish "./eec-backend/eec-backend.csproj" -c release -o /app --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:6.0-alpine as runtime
WORKDIR /app
COPY --from=build /app ./

EXPOSE 5000

ENTRYPOINT [ "dotnet", "eec-backend.dll" ]