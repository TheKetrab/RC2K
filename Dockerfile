# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /app
COPY ["src/RC2K.Presentation.Blazor/RC2K.Presentation.Blazor.csproj", "src/RC2K.Presentation.Blazor/"]
COPY ["src/RC2K.DataAccess.Database/RC2K.DataAccess.Database.csproj", "src/RC2K.DataAccess.Database/"]
COPY ["src/RC2K.DataAccess.Interfaces/RC2K.DataAccess.Interfaces.csproj", "src/RC2K.DataAccess.Interfaces/"]
COPY ["src/RC2K.DomainModel/RC2K.DomainModel.csproj", "src/RC2K.DomainModel/"]
COPY ["src/RC2K.Resources/RC2K.Resources.csproj", "src/RC2K.Resources/"]
COPY ["src/RC2K.Extensions/RC2K.Utils.csproj", "src/RC2K.Extensions/"]
COPY ["src/RC2K.DataAccess.Dynamic/RC2K.DataAccess.Dynamic.csproj", "src/RC2K.DataAccess.Dynamic/"]
COPY ["src/RC2K.Logic.Interfaces/RC2K.Logic.Interfaces.csproj", "src/RC2K.Logic.Interfaces/"]
COPY ["src/RC2K.Logic/RC2K.Logic.csproj", "src/RC2K.Logic/"]
RUN dotnet restore "./src/RC2K.Presentation.Blazor/RC2K.Presentation.Blazor.csproj"
COPY . .
WORKDIR "/app/src/RC2K.Presentation.Blazor"
RUN dotnet build "./RC2K.Presentation.Blazor.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./RC2K.Presentation.Blazor.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY ["data/RC2K.db", "."]
ENTRYPOINT ["dotnet", "RC2K.Presentation.Blazor.dll"]