FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS build
WORKDIR /src

RUN apk add --no-cache icu-libs=76.1-r1
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false

COPY nuget.config ./
COPY packages ./packages

COPY Pagamentos.Api/Pagamentos.Api.csproj ./Pagamentos.Api/
RUN dotnet restore ./Pagamentos.Api/Pagamentos.Api.csproj

COPY Pagamentos.Api/. ./Pagamentos.Api/

RUN dotnet publish ./Pagamentos.Api/Pagamentos.Api.csproj -c Release -o /app/publish --no-restore --no-self-contained

FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine AS final
WORKDIR /app

RUN apk add --no-cache icu-libs=76.1-r1
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false
ENV ASPNETCORE_URLS=http://+:80

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "Pagamentos.Api.dll"]