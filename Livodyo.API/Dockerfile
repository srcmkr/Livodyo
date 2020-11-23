# Starting with .db-File routing: docker run -d -p 8080:80 -v /usr/share/livodyo/Livodyo.db:/app/Livodyo.db:Z srcmkr/livodyoapi:latest
# Starting webadmin with: docker run -d -p 8081:80 -e API="http://hsh2brain.privacy.ltd:8080" srcmkr/livodyowebadmin:latest
# Visit http://hsh2brain.privacy.ltd:8081 for admin
# visit http://hsh2brain.privacy.ltd:8080 for HTTP/JSON API

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