1. Go to repo root.
docker build -t theketrab/rc2k-hub-image .
docker push theketrab/rc2k-hub-image
docker run --name rc2k-hub -d -e "Security__Salt=SECRET" -e "Security__Iterations=SECRET" -e "Cosmos__ApiKey=SECRET" -e "ASPNETCORE_ENVIRONMENT=Release" -e "APPLICATIONINSIGHTS_CONNECTION_STRING=SECRET" -p 5005:8080 rc2k-hub-image