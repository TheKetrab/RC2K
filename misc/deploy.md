
# DEV DEPLOY
docker build -t theketrab/rc2k-hub-image .
docker push theketrab/rc2k-hub-image
docker run --name rc2k-hub -d \
  -e "Security__Salt=SECRET" \
  -e "Security__Iterations=SECRET" \
  -e "Cosmos__ApiKey=SECRET" \
  -e "Mailing__SftpAppPassword=SECRET" \
  -e "Mailing__SenderEmail=SECRET"
  -e "ASPNETCORE_ENVIRONMENT=Development" \
  -e "APPLICATIONINSIGHTS_CONNECTION_STRING=SECRET" \
  -p 5005:8080 rc2k-hub-image

# PROD
docker build -t theketrab/rc2k-hub-prod-image .
docker push theketrab/rc2k-hub-prod-image
docker run --name rc2k-hub-prod -d \
  -e "Security__Salt=SECRET" \
  -e "Security__Iterations=SECRET" \
  -e "Mailing__SftpAppPassword=SECRET" \
  -e "Mailing__SenderEmail=SECRET"
  -e "Cosmos__ApiKey=SECRET" \
  -e "ASPNETCORE_ENVIRONMENT=Production" \
  -e "APPLICATIONINSIGHTS_CONNECTION_STRING=SECRET" \
  -p 5005:8080 rc2k-hub-prod-image
