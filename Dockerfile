FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 5022
ENV ASPNETCORE_URLS http://+:5022

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["Api/Api.csproj", "Api/"]
COPY ["Application/Application.csproj", "Application/"]
COPY ["Domain/Domain.csproj", "Domain/"]
COPY ["Persistence/Persistence.csproj", "Persistence/"]
COPY ["Infrastructure/Infrastructure.csproj", "Infrastructure/"]
COPY ["UdpServer/UdpServer.csproj", "UdpServer/"]


RUN dotnet restore "Api/Api.csproj"
COPY . .
WORKDIR "/src/Api" ASPNETCORE_ENVIRONMENT=Production
ENV ASPNETCORE_ENVIRONMENT=Production
ENV PATH="$PATH:/root/.dotnet/tools"
RUN dotnet build "Api.csproj" -c Release -o /app/build
RUN dotnet tool install --global dotnet-ef --version 3.1.8
RUN  ASPNETCORE_ENVIRONMENT=Production dotnet ef migrations add MysqlDockerMigrations -p ../Persistence -s .

FROM build AS publish

RUN dotnet publish "Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Api.dll"]
