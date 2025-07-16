#!/bin/bash

# Build script for Render deployment
echo "Building Vidro API..."

# Restore dependencies
dotnet restore vidro.api/vidro.api.csproj

# Build the application
dotnet build vidro.api/vidro.api.csproj -c Release

echo "Build completed successfully!"