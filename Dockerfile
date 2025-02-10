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

RUN dotnet build "TalentConsulting.TalentSuite.Projects.sln"

WORKDIR "/src/TalentConsulting.TalentSuite.Projects.API"

# Build the project
RUN dotnet publish -c Release -o /app/publish --no-restore

# Final stage
FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .

# Expose necessary ports (change as needed)
EXPOSE 80
EXPOSE 443

# Run the application
ENTRYPOINT ["dotnet", "TalentConsulting.TalentSuite.Projects.API.dll"]






# # Build Stage One - Select a subset of files in repository so that we can dotnet restore
# FROM mcr.microsoft.com/dotnet/sdk:8.0 AS restore-env
# ENV PATH="${PATH}:/root/.dotnet/tools"
# RUN dotnet tool install --global --no-cache dotnet-subset
# WORKDIR /restore
# COPY . /restore/Applications

# RUN dotnet subset restore Applications/TalentConsulting.TalentSuite.Projects.API/TalentConsulting.TalentSuite.Projects.API.csproj --root-directory /restore --output restore_subset/

# # Build Stage Two - Execute the restore (which will be cached if no changes detected above) and build
# FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
# WORKDIR /src
# COPY --from=restore-env /restore/restore_subset .
# RUN dotnet restore /src/Applications/TalentConsulting.TalentSuite.Projects.API/TalentConsulting.TalentSuite.Projects.API.csproj

# # Stage Three - Now we copy ALL the source code and run a full build
# # COPY .Applications /src/Applications
# # RUN dotnet publish Applications/MyApp/MyApp.csproj -c Release -o out
# RUN dotnet build ./Applications/TalentConsulting.TalentSuite.Projects.API/TalentConsulting.TalentSuite.Projects.API.csproj -c Release -o out

# # # Build runtime image
# # FROM mcr.microsoft.com/dotnet/aspnet:7.0
# # WORKDIR /app
# # COPY --from=build-env /src/out .
# # ENTRYPOINT ["/app/MyApp"]



# # FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
# # WORKDIR /app
# # EXPOSE 80
# # EXPOSE 443

# # FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
# # WORKDIR /src
# # COPY . .
# # RUN dotnet restore "./TalentConsulting.TalentSuite.Projects.sln"
# # COPY . .
# # WORKDIR "/src/TalentConsulting.TalentSuite.Projects.API/."

# # RUN dotnet build "TalentConsulting.TalentSuite.Projects.API.csproj" -c Release -o /app/build

# # FROM build AS publish
# # RUN dotnet publish "TalentConsulting.TalentSuite.Projects.API.csproj" -c Release -o /app/publish

# # FROM base AS final
# # WORKDIR /app
# # COPY --from=publish /app/publish .

# # CMD ["./wait-for-it.sh", "sqlserver:1433", "--", "dotnet", "TalentConsulting.TalentSuite.Projects.API.dll"]

# # # Use .NET SDK to build the app
# # FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
# # WORKDIR /app
# # COPY . .
# # RUN dotnet restore "./TalentConsulting.TalentSuite.Projects.sln"

# # WORKDIR "/src/TalentConsulting.TalentSuite.Projects.API/."

# # RUN dotnet build "./TalentConsulting.TalentSuite.Projects.API/TalentConsulting.TalentSuite.Projects.API.csproj" -c Release -o /out

# # RUN dotnet publish "TalentConsulting.TalentSuite.Projects.API/TalentConsulting.TalentSuite.Projects.API.csproj" -c Release -o /out/publish

# # # Use runtime image for final container
# # FROM mcr.microsoft.com/dotnet/aspnet:8.0
# # WORKDIR /app
# # COPY --from=build /out/publish .
# # ENTRYPOINT ["dotnet", "TalentConsulting.TalentSuite.Projects.API.dll"]