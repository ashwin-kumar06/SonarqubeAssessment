# Use the official .NET 7 SDK image as a build image.
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build

# Set the working directory.
WORKDIR /app

# Copy the project file.
COPY *.csproj ./

# Restore the dependencies.
RUN dotnet restore

# Copy the remaining files.
COPY . ./

# Build the application.
RUN dotnet publish -c Release -o out

# Use the official .NET 7 runtime image as a runtime image.
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS runtime

# Set the working directory.
WORKDIR /app

# Copy the published files.
COPY --from=build /app/out ./

# Set the environment variables.
ENV ConnectionStrings__DefaultConnection="server=localhost;database=employeemanageapi;user=root;password=root"

# Expose the port.
EXPOSE 80

# Start the application.
ENTRYPOINT ["dotnet", "EmployeeManagementApi.dll"]
