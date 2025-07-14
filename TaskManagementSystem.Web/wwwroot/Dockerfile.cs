# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy csproj and restore
COPY ["TaskManagementSystem.Web/TaskManagementSystem.Web.csproj", "TaskManagementSystem.Web/"]
COPY ["TaskManagementSystem.Application/TaskManagementSystem.Application.csproj", "TaskManagementSystem.Application/"]
COPY ["TaskManagementSystem.Infrastructure/TaskManagementSystem.Infrastructure.csproj", "TaskManagementSystem.Infrastructure/"]
COPY ["TaskManagementSystem.DataAccess/TaskManagementSystem.DataAccess.csproj", "TaskManagementSystem.DataAccess/"]
COPY ["TaskManagementSystem.Domain/TaskManagementSystem.Domain.csproj", "TaskManagementSystem.Domain/"]
# Add other project files if necessary

RUN dotnet restore "TaskManagementSystem.Web/TaskManagementSystem.Web.csproj"

# Copy the rest of the source code
COPY . .

WORKDIR "/src/TaskManagementSystem.Web"
RUN dotnet publish -c Release -o /app/publish

# Stage 2: Run
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "TaskManagementSystem.Web.dll"]
