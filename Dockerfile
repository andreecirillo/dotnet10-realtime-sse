FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["apps/OrderStreaming.Api/OrderStreaming.Api.csproj", "apps/OrderStreaming.Api/"]
COPY ["core/OrderStreaming.Domain/OrderStreaming.Domain.csproj", "core/OrderStreaming.Domain/"]
RUN dotnet restore "apps/OrderStreaming.Api/OrderStreaming.Api.csproj"

COPY . .
RUN dotnet publish "apps/OrderStreaming.Api/OrderStreaming.Api.csproj" -c Release -o /app/api

FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app
COPY --from=build /app/api .
ENV ASPNETCORE_URLS=http://+:8081
EXPOSE 8081
ENTRYPOINT ["dotnet", "OrderStreaming.Api.dll"]