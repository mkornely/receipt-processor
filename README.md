
# Instructions
This application was built on .NET 8 using C# and JetBrains Rider.
The application is a RESTful API that processes receipts and awards points based on the rules provided in the problem statement (https://github.com/fetch-rewards/receipt-processor-challenge/blob/main/README.md) .
The application uses an in-memory Dictionary to store the receipt IDs (GUID) and their corresponding points.

## Running the application
This solution can be run using the following methods:

### Using JetBrains Rider or Visual Studio
1. Open the solution in the IDE
2. Run and compile the application to expose the API endpoints and swagger interface
3. Swagger should launch automatically in the browser

### Using Docker
1. Build the Docker image and start the container using the following command in the root directory of the solution (./Receipt-Processor):
```shell
docker-compose up --build
```
2. The application will be available on `http://localhost:8080` and will accept payloads 
```json
curl -X 'POST' \
'http://localhost:8080/receipts' \
-H 'accept: text/plain' \
-H 'Content-Type: application/json' \
-d '{
"retailer": "2yfe&Dw1kO-q3wawp",
"purchaseDate": "2024-12-03",
"purchaseTime": "14:24",
"items": [
{
"shortDescription": "I",
"price": "0720852111099317390771380094404787450726701806594346675789290626428612918968.71"
}
],
"total": "7.62"
}'
```

3. The swagger interface will be available on `http://localhost:8080/swagger/index.html` where you can test these payloads and have a more detailed view of the web service
