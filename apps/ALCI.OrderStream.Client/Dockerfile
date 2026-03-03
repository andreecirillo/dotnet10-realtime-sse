FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["apps/ALCI.OrderStream.Client/ALCI.OrderStream.Client.csproj", "apps/ALCI.OrderStream.Client/"]
COPY ["core/ALCI.OrderStream.Domain/ALCI.OrderStream.Domain.csproj", "core/ALCI.OrderStream.Domain/"]
COPY ["tools/tailwind/tailwindcss-linux-x64", "tools/tailwind/"]
RUN chmod +x tools/tailwind/tailwindcss-linux-x64

RUN dotnet restore "apps/ALCI.OrderStream.Client/ALCI.OrderStream.Client.csproj"

COPY . .
RUN dotnet publish "apps/ALCI.OrderStream.Client/ALCI.OrderStream.Client.csproj" -c Release -o /app/client

FROM nginx:alpine
WORKDIR /usr/share/nginx/html
COPY --from=build /app/client/wwwroot .

RUN echo 'server { \
    listen 8080; \
    location / { \
        root /usr/share/nginx/html; \
        try_files $uri $uri/ /index.html; \
    } \
}' > /etc/nginx/conf.d/default.conf

EXPOSE 8080