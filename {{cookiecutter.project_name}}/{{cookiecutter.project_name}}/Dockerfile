FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5054

ENV ASPNETCORE_URLS=http://+:5054

USER app
FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG configuration=Release
WORKDIR /src
COPY ["{{cookiecutter.project_name}}/{{cookiecutter.project_name}}.csproj", "{{cookiecutter.project_name}}/"]
RUN dotnet restore "{{cookiecutter.project_name}}/{{cookiecutter.project_name}}.csproj"
COPY . .
WORKDIR "/src/{{cookiecutter.project_name}}"
RUN dotnet build "{{cookiecutter.project_name}}.csproj" -c $configuration -o /app/build

FROM build AS publish
ARG configuration=Release
RUN dotnet publish "{{cookiecutter.project_name}}.csproj" -c $configuration -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "{{cookiecutter.project_name}}.dll"]
