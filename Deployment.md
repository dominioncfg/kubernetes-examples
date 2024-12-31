# Deployment Guide

## Introduction

This guide provides a step-by-step walkthrough for deploying a full-stack application to a Kubernetes cluster. The application is composed of the following components:

1. **SQL Server Database:** The persistent data storage solution.
2. **.NET Core Web API:** A backend service that handles business logic and interacts with the database.
3. **Database Migration Job:** A utility for deploying and applying database schema migrations.
4. **Nginx Frontend:** A web server hosting the client-facing interface.

The deployment process leverages Kubernetes features such as pods, services, and ingress controllers to orchestrate and expose the application. By following this guide, you'll learn how to:

* Build and containerize each application component.
* Configure and deploy Kubernetes resources for a seamless deployment.
* Apply database migrations to ensure the database schema is ready for the backend API.
* Set up an Nginx ingress controller to manage routing and load balancing.

The guide is tested with minikube in mind. 

## Deploy the Database

### Apply Database Deployment
```bash
kubectl apply -f Infra/database.yaml
```

## Apply Database Migrations

### Build Database Migrator Image Image
```bash
docker build -t students-db-migrator:v1.3 -f KubernetesExample.DbMigratorRunner/Dockerfile ..
```

### Load Database Migrator Image to Minikube Cluster
```bash
minikube image load students-db-migrator:v1.3
```

### Deploy Migrations Job
```bash
kubectl apply -f Infra/database-migrator.yaml
```

## Deploy the Backend

### Build Backend Image
```bash
docker build -t students-api:v1.3 -f KubernetesExample/Dockerfile .
```

### Load Backend Image to Minikube Cluster
```bash
minikube image load students-api:v1.3
```

### Apply Backend Deployment
```bash
kubectl apply -f Infra/backend.yaml
```

## Deploy the Frontend

### Build Frontend Image
```bash
docker build -t frontend-server:v1.3 -f Infra/Html/Dockerfile Infra/html
```

### Load Frontend Image into the Minikube Cluster
```bash
minikube image load frontend-server:v1.3
```

### Apply Frontend Deployment
```bash
kubectl apply -f Infra/frontend.yaml
```

---

## Deploy Ingress Routes

### Setup and Nginx Ingress Controller
Use the script located in `Examples/13.Ingress`. Only the Controller is needed

### Deploy the Routes
```bash
kubectl apply -f Infra/ingress-routes.yaml
```

### Deploy the Nginx Ingress
```bash
minikube service ingress-nginx-controller --url  -n ingress-nginx
```


### Open the frontend App
---
The App should be listening in `http://127.0.0.1:XXXXXX/app`

## Clean up all resources after you finish
```bash
kubectl delete -f Infra/ingress-routes.yaml
kubectl delete -f Infra/frontend.yaml
kubectl delete -f Infra/backend.yaml
kubectl delete -f Infra/database-migrator.yaml
kubectl delete -f Infra/database.yaml
```

---
## Other scripts

### Run a Pod Inside the Cluster and Connect to the Database

#### Run MSSQL Tools in the Cluster to Test the Database
```bash
kubectl run -i --tty --rm debug --image=mcr.microsoft.com/mssql-tools:latest --restart=Never
```

#### Connect to the Database
```sql
sqlcmd -S data-base-service -U sa -P PataDeCabra@2020
SELECT name, database_id, create_date FROM sys.databases;
GO
```

---

### For Local Development

### Run SQL Server Image
```bash
docker run -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=PataDeCabra@2020" -p 1433:1433 -d mcr.microsoft.com/mssql/server:2022-latest
```

### Test Locally Docker Image
```bash
docker run --network host -d students-api:v1.3
```

### Add Migration
```bash
dotnet ef migrations add InitialMigration --project KubernetesExample.SharedDataStorage  --startup-project KubernetesExample.DbMigratorRunner -c AppDbContext -o Migrations
```
