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
