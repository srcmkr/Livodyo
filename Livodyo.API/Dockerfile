#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src
COPY ["Livodyo.API/Livodyo.API.csproj", "Livodyo.API/"]
RUN dotnet restore "Livodyo.API/Livodyo.API.csproj"
COPY . .
WORKDIR "/src/Livodyo.API"
RUN dotnet build "Livodyo.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Livodyo.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Livodyo.API.dll"]