# Vidro API - Render Deployment

This .NET 8 Web API application is configured for deployment on Render.

## Local Development

1. Install .NET 8 SDK
2. Install PostgreSQL or use Docker Compose
3. Run the application:
   ```bash
   dotnet run --project vidro.api
   ```

## Deployment to Render

### Prerequisites
- GitHub repository with this code
- Render account

### Deployment Steps

1. **Fork/Push this repository to GitHub**

2. **Create a new Web Service on Render:**
   - Connect your GitHub repository
   - Use the following settings:
     - **Environment**: Docker
     - **Dockerfile Path**: `./vidro.api/Dockerfile`
     - **Docker Context**: `.` (root directory)

3. **Set Environment Variables in Render:**
   ```
   ASPNETCORE_ENVIRONMENT=Production
   ASPNETCORE_URLS=http://0.0.0.0:$PORT
   ```

4. **Create PostgreSQL Database:**
   - Create a new PostgreSQL database on Render
   - Note the connection string provided by Render

5. **Configure Database Connection:**
   - Add the environment variable `ConnectionStrings__VidroConnection` with your Render PostgreSQL connection string
   - Or use the automatic database linking in render.yaml

6. **Deploy:**
   - Push your code to GitHub
   - Render will automatically build and deploy your application

### Using render.yaml (Recommended)

This repository includes a `render.yaml` file for Infrastructure as Code deployment:

1. Go to Render Dashboard
2. Create a new "Blueprint"
3. Connect your GitHub repository
4. Render will automatically create both the web service and PostgreSQL database

## Environment Variables

- `ASPNETCORE_ENVIRONMENT` - Set to "Production" for production deployment
- `ASPNETCORE_URLS` - Set to `http://0.0.0.0:$PORT` for Render
- `ConnectionStrings__VidroConnection` - PostgreSQL connection string

## Notes

- The application will automatically create the database schema on first run
- Swagger UI is only available in development mode
- CORS is configured for production with specific origins (update in Program.cs)
- Health checks are available at `/health` endpoint