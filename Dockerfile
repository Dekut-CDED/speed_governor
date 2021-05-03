FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build

WORKDIR /src
COPY ["Api/Api.csproj", "Api/"]
COPY ["Application/Application.csproj", "Application/"]
COPY ["Domain/Domain.csproj", "Domain/"]
COPY ["Persistence/Persistence.csproj", "Persistence/"]
COPY ["Infrastructure/Infrastructure.csproj", "Infrastructure/"]

RUN dotnet restore "Api/Api.csproj"
COPY . .
WORKDIR "/src/Api" 
ENV ASPNETCORE_ENVIRONMENT=Production
ENV PATH="$PATH:/root/.dotnet/tools"
RUN dotnet build "Api.csproj" -c Release -o /app/build
RUN dotnet tool install --global dotnet-ef --version 3.1.8
RUN dotnet publish "Api.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:5.0
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 5022
EXPOSE 5021
EXPOSE 3030
ENV ASPNETCORE_URLS http://+:5022
ENTRYPOINT ["dotnet", "Api.dll"]
