# Base Image
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80 443


# Copy Solution File to support Multi-Project
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY TalentConsulting.TalentSuite.Projects.API.sln ./

# Copy Dependencies
COPY ["src/TalentConsulting.TalentSuite.Projects.API/TalentConsulting.TalentSuite.Projects.API.csproj", "src/TalentConsulting.TalentSuite.Projects.API/"]
COPY ["src/TalentConsulting.TalentSuite.Projects.Core/TalentConsulting.TalentSuite.Projects.Core.csproj", "src/TalentConsulting.TalentSuite.Projects.Core/"]
COPY ["src/TalentConsulting.TalentSuite.Projects.Infrastructure/TalentConsulting.TalentSuite.Projects.Infrastructure.csproj", "src/TalentConsulting.TalentSuite.Projects.Infrastructure/"]
COPY ["src/TalentConsulting.TalentSuite.Projects.Common/TalentConsulting.TalentSuite.Projects.Common.csproj", "src/TalentConsulting.TalentSuite.Projects.Common/"]

# Restore Project
RUN dotnet restore "src/TalentConsulting.TalentSuite.Projects.API/TalentConsulting.TalentSuite.Projects.API.csproj"

# Copy Everything
COPY . .

# Build
WORKDIR "/src/src/TalentConsulting.TalentSuite.Projects.API"
RUN dotnet build "TalentConsulting.TalentSuite.Projects.API.csproj" -c Release -o /app/build

# publish
FROM build AS publish
WORKDIR "/src/src/TalentConsulting.TalentSuite.Projects.API"
RUN dotnet publish "TalentConsulting.TalentSuite.Projects.API.csproj" -c Release -o /app/publish

# Build runtime image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TalentConsulting.TalentSuite.Projects.API.dll"]