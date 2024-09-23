# Use the official .NET SDK image to build the api
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy the .csproj file and restore dependencies
COPY api/*.csproj ./
RUN dotnet restore

# Copy the entire project and build the release
COPY ./api ./
RUN dotnet publish -c Release -o out

# Use the official .NET runtime image for the api
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/out .

# Install Nginx
RUN apt-get update && \
    apt-get install -y nginx && \
    rm -rf /var/lib/apt/lists/*

RUN rm /etc/nginx/sites-enabled/default
RUN rm /etc/nginx/nginx.conf
COPY nginx.conf /etc/nginx/sites-available/default
RUN ln -s /etc/nginx/sites-available/default /etc/nginx/sites-enabled/default
COPY nginx.conf /etc/nginx

# Set environment variables
ENV ASPNETCORE_URLS=http://+:5043

# Expose the port on which the api will run
EXPOSE 80 5043

# Run the application
CMD ["sh", "-c", "nginx && dotnet api.dll"]
# ENTRYPOINT ["dotnet", "api.dll"]
