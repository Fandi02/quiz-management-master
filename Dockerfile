### PREPARE
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build-env
WORKDIR /src

### Copy csproj and sln and restore as distinct layers
COPY QuizManagement.Web/*.csproj QuizManagement.Web/
COPY QuizManagement.Api/*.csproj QuizManagement.Api/
COPY QuizManagement.Shared/*.csproj QuizManagement.Shared/
COPY QuizManagement.sln .
RUN dotnet restore

### PUBLISH
FROM build-env as publish-env
COPY . .
RUN dotnet publish "QuizManagement.sln" -c Release -o /app

### RUNTIME IMAGE
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS runtime-env
WORKDIR /app
COPY --from=publish-env /app .
ENV ASPNETCORE_URLS=http://+:80
EXPOSE 80
ENTRYPOINT ["dotnet", "QuizManagement.Api.dll"]