# MicroservicesLearning

## Docker setup
Postman
   │
   │ HTTP localhost:7004
   ▼
┌──────────────────────┐
│ OrderService         │
│ container :8080      │
└──────────┬───────────┘
           │
           │ HTTP
           │ productservice-container:8080
           ▼
┌──────────────────────┐
│ ProductService       │
│ container :8080      │
└──────────────────────┘



## Docker commands

### Stop and remove the old container
```powershell
docker stop orderservice-container
docker rm orderservice-container
```

### Start the new container
```powershell
docker run -d `
  --name orderservice-container `
  --network microservices-network `
  -p 7004:8080 `
  -e "Services__ProductService=http://productservice-container:8080" `
  orderservice:latest
```
