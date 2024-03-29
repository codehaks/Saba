#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.hamdocker.ir/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.hamdocker.ir/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/Saba.Web/Saba.Web.csproj", "src/Saba.Web/"]
RUN dotnet restore "src/Saba.Web/Saba.Web.csproj"
COPY . .
WORKDIR "/src/src/Saba.Web"
RUN dotnet build "Saba.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Saba.Web.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Saba.Web.dll"]