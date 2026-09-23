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

###Start the new container

docker run -d `
  --name orderservice-container `
  --network microservices-network `
  -p 7004:8080 `
  -e "Services__ProductService=http://productservice-container:8080" `
  orderservice:latest
  

````markdown

### Stop and remove the old container
```bash
docker stop orderservice-container
docker rm orderservice-container
```

### Start the new container


```bash
docker run -d `
  --name orderservice-container `
  --network microservices-network `
  -p 7004:8080 `
  -e "Services__ProductService=http://productservice-container:8080" `
  orderservice:latest
```


````