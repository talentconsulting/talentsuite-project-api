FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy the solution file and restore as a separate step for better caching
COPY ["./src/TalentConsulting.TalentSuite.Projects.sln", "./"]
COPY ["./src/TalentConsulting.TalentSuite.Projects.API/TalentConsulting.TalentSuite.Projects.API.csproj", "./TalentConsulting.TalentSuite.Projects.API/"]
COPY ["./src/TalentConsulting.TalentSuite.Projects.Common/TalentConsulting.TalentSuite.Projects.Common.csproj", "./TalentConsulting.TalentSuite.Projects.Common/"]
COPY ["./src/TalentConsulting.TalentSuite.Projects.Core/TalentConsulting.TalentSuite.Projects.Core.csproj", "./TalentConsulting.TalentSuite.Projects.Core/"]
COPY ["./src/TalentConsulting.TalentSuite.Projects.Infrastructure/TalentConsulting.TalentSuite.Projects.Infrastructure.csproj", "./TalentConsulting.TalentSuite.Projects.Infrastructure/"]

RUN dotnet restore "TalentConsulting.TalentSuite.Projects.sln"

# Copy the rest of the source code
COPY ./src .
COPY ./wait-for-it.sh .
RUN chmod +x wait-for-it.sh

RUN dotnet build "TalentConsulting.TalentSuite.Projects.sln"

WORKDIR "/src/TalentConsulting.TalentSuite.Projects.API"

# Build the project
RUN dotnet publish -c Release -o /app/publish --no-restore

# Final stage
FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
COPY --from=build /src/wait-for-it.sh .

# Expose necessary ports (change as needed)
EXPOSE 80
EXPOSE 443

# Run the application
ENTRYPOINT ["/app/wait-for-it.sh", "sqlserver:1433", "-s", "-t", "20", "--", "dotnet", "TalentConsulting.TalentSuite.Projects.API.dll"]