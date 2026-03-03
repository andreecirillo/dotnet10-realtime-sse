FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["apps/ALCI.OrderStream.API/ALCI.OrderStream.API.csproj", "apps/ALCI.OrderStream.API/"]
COPY ["core/ALCI.OrderStream.Domain/ALCI.OrderStream.Domain.csproj", "core/ALCI.OrderStream.Domain/"]
RUN dotnet restore "apps/ALCI.OrderStream.API/ALCI.OrderStream.API.csproj"

COPY . .
RUN dotnet publish "apps/ALCI.OrderStream.API/ALCI.OrderStream.API.csproj" -c Release -o /app/api

FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app
COPY --from=build /app/api .
ENV ASPNETCORE_URLS=http://+:8081
EXPOSE 8081

ENTRYPOINT ["dotnet", "ALCI.OrderStream.API.dll"]