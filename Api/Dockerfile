﻿ARG dotnet_version=8.0
########################################
#  First stage of multistage build
########################################
#  Use Build image with label `builder
########################################
FROM mcr.microsoft.com/dotnet/sdk:${dotnet_version}-alpine AS builder
# Project file name, mandatory parameter, e.g. Helpdesk.Api
ARG project_name=Api
ARG run_codegen=true

# Setup working directory for project
WORKDIR /app

COPY ./${project_name}.csproj ./

# Restore nuget packages
RUN dotnet restore ./${project_name}.csproj

# Copy project files
COPY ./ ./

# Run code generation depending on the build argument
RUN if [ "${run_codegen}" = true ] ; then dotnet run -- codegen write & dotnet run -- codegen test; else echo "skipping code generation"; fi

# Build project with Release configuration
# and no restore, as we did it already
RUN dotnet build -c Release --no-restore ./${project_name}.csproj

# Publish project to output folder
# and no build, as we did it already
WORKDIR /app/
RUN ls
RUN dotnet publish -c Release --no-build -o out

########################################
#  Second stage of multistage build
########################################
#  Use other build image as the final one
#    that won't have source codes
########################################
FROM mcr.microsoft.com/dotnet/aspnet:${dotnet_version}-alpine
ARG project_name=Api
ARG dotnet_version=8.0

# Setup working directory for project
WORKDIR /app

# Copy published in previous stage binaries
# from the `builder` image
COPY --from=builder /app/out .

# Set URL that App will be exposed
ENV ASPNETCORE_URLS="http://*:80"
ENV PROJECT_DLL="${project_name}.dll"

# sets entry point command to automatically
# run application on `docker run`
ENTRYPOINT dotnet $PROJECT_DLL