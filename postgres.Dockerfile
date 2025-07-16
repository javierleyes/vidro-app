FROM postgres:15

# Copy any initialization scripts if they exist
COPY vidro.database/init.sql /docker-entrypoint-initdb.d/ 2>/dev/null || true

EXPOSE 5432