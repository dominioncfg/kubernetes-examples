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

### 1.1 Apply Database Deployment
```bash
kubectl apply -f Infra/database.yaml
```

## 2. Deploy the Backend

### 2.1 Build Backend Image
```bash
docker build -t students-api:v1.3 -f KubernetesExample/Dockerfile .
```

### 2.2 Load Backend Image to Minikube Cluster
```bash
minikube image load students-api:v1.3
```

### 2.3 Apply Backend Deployment
```bash
kubectl apply -f Infra/backend.yaml
```

## 3. Deploy the Frontend

### 3.1 Build Frontend Image
```bash
docker build -t frontend-server:v1.3 -f Infra/Html/Dockerfile Infra/html
```

### 3.2 Load Frontend Image into the Minikube Cluster
```bash
minikube image load frontend-server:v1.3
```

### 3.3 Apply Frontend Deployment
```bash
kubectl apply -f Infra/frontend.yaml
```

---

## 4. Deploy Ingress Routes

### 4.1 Setup and Nginx Ingress Controller
Use the script located in `Examples/13.Ingress`.

### 4.2 Deploy the Routes
```bash
kubectl apply -f Infra/ingress-routes.yaml
```

---

## Clean up all resources after you finish
```bash
kubectl delete -f Infra/database.yaml
kubectl delete -f Infra/backend.yaml
kubectl delete -f Infra/frontend.yaml
kubectl delete -f Infra/ingress-routes.yaml
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

### 6. For Local Development

### Run SQL Server Image
```bash
docker run -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=PataDeCabra@2020" -p 1433:1433 -d mcr.microsoft.com/mssql/server:2022-latest
```

### Test Locally Docker Image
```bash
docker run --network host -d students-api:v1.3
```
