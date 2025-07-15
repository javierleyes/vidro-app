# Stop all running containers
docker ps -q | ForEach-Object { docker stop $_ }

# Remove all containers
docker ps -aq | ForEach-Object { docker rm $_ }

# Remove all images
docker images -q | ForEach-Object { docker rmi -f $_ }

# Remove all volumes
docker volume ls -q | ForEach-Object { docker volume rm $_ }

Write-Host "All containers, images, and volumes have been removed."