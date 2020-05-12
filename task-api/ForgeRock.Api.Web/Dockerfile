#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["ForgeRock.Api.Web/ForgeRock.Api.Web.csproj", "ForgeRock.Api.Web/"]
RUN dotnet restore "ForgeRock.Api.Web/ForgeRock.Api.Web.csproj"
COPY . .
WORKDIR "/src/ForgeRock.Api.Web"
RUN dotnet build "ForgeRock.Api.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ForgeRock.Api.Web.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ForgeRock.Api.Web.dll"]