# Build from the repository root so the shared Directory.*.props and global.json are in the context:
#   docker build -f 021-Containers-Docker-and-Docker-Compose/NotesApi/Dockerfile -t notes-api .
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY global.json Directory.Build.props Directory.Packages.props ./
COPY 021-Containers-Docker-and-Docker-Compose/NotesApi/NotesApi.csproj NotesApi/
RUN dotnet restore NotesApi/NotesApi.csproj
COPY 021-Containers-Docker-and-Docker-Compose/NotesApi/ NotesApi/
RUN dotnet publish NotesApi/NotesApi.csproj -c Release -o /app --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app
COPY --from=build /app .
USER app
EXPOSE 8080
ENTRYPOINT ["dotnet", "NotesApi.dll"]
