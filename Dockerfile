#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:8.0-buster-slim AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:8.0-buster AS build
WORKDIR /src
COPY ["src/XBLMS.Web/XBLMS.Web.csproj", "src/XBLMS.Web/"]
COPY ["src/XBLMS.Core/XBLMS.Core.csproj", "src/XBLMS.Core/"]
COPY ["src/XBLMS/XBLMS.csproj", "src/XBLMS/"]
RUN dotnet restore "src/XBLMS.Web/XBLMS.Web.csproj"
COPY . .
WORKDIR "/src/src/XBLMS.Web"
RUN dotnet build "XBLMS.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "XBLMS.Web.csproj" -c Release -o /app/xblms
RUN cp -r /app/xblms/wwwroot /app/xblms/_wwwroot
RUN echo `date +%Y-%m-%d-%H-%M-%S` > /app/xblms/_wwwroot/sitefiles/version.txt

FROM base AS final
WORKDIR /app
COPY --from=publish /app/xblms .
ENTRYPOINT ["dotnet", "XBLMS.Web.dll"]

# docker build -t xblms/core:dev .