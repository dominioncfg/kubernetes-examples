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

---
## Create the Registry

### Run a local registry
```bash
docker-compose --file Infra/Local/docker-compose.yml up -d
```

### Login our docker host with the container to be able to push images
```bash
docker login -u jc -p Patata@1 localhost:5000   
```

### Setup a secret in kubernetes to be able to pull from the container
```bash
kubectl apply -f Infra/Local/auth-secrets.yaml
```

### Use the secret when pulling images automatically by changing the Service Account
```bash
kubectl patch serviceaccount default -p '{"imagePullSecrets": [{"name": "local-docker-registry"}]}'
```

---

## Add Insecure Registry to Minikube

### Stop minikube in case is running
```bash
minikube stop
```

### Configure the local registry as insecure
```bash
minikube config set insecure-registry "host.docker.internal:5000"
```

### Start minikube
```bash
minikube start
```

### Check if contianer is accessible

```bash
minikube ssh -- curl -u jc:Patata@1 http://host.docker.internal:5000/v2/_catalog
```

---
## Deploy the Database

### Apply Database Deployment
```bash
kubectl apply -f Infra/database.yaml
```

## Apply Database Migrations

### Build Database Migrator Image Image
```bash
docker build -t localhost:5000/students-db-migrator:v1.3 -f KubernetesExample.DbMigratorRunner/Dockerfile .
```


### Push The Image to the local repository
```bash
docker push localhost:5000/students-db-migrator:v1.3
```

### Deploy Migrations Job
```bash
kubectl apply -f Infra/database-migrator.yaml
```

---

## Deploy the Backend

### Build Backend Image
```bash
docker build -t localhost:5000/students-api:v1.3 -f KubernetesExample/Dockerfile .
```

###  Push The Image to the local repository
```bash
docker push localhost:5000/students-api:v1.3
```

### Apply Backend Deployment
```bash
kubectl apply -f Infra/backend.yaml
```

---

## Deploy the Frontend

### Build Frontend Image
```bash
docker build -t localhost:5000/frontend-server:v1.3 -f Infra/Html/Dockerfile Infra/html
```

### Push The Image to the local repository
```bash
docker push localhost:5000/frontend-server:v1.3
```

### Apply Frontend Deployment
```bash
kubectl apply -f Infra/frontend.yaml
```

---

## Deploy Ingress Routes

### Setup and Nginx Ingress Controller
```bash
kubectl apply -f Infra/ngnix-controller.yaml
```
### Deploy the Routes
```bash
kubectl apply -f Infra/ingress-routes.yaml
```

### Expose the Nginx Ingress (Only requireded in Minikube)
```bash
minikube service ingress-nginx-controller --url  -n ingress-nginx
```


### Open the frontend App
---
The App should be listening in `http://127.0.0.1:XXXXX/app`

## Clean up all resources after you finish
```bash
kubectl delete -f Infra/ingress-routes.yaml
kubectl delete -f Infra/frontend.yaml
kubectl delete -f Infra/backend.yaml
kubectl delete -f Infra/database-migrator.yaml
kubectl delete -f Infra/database.yaml
kubectl delete -f Infra/Local/auth-secrets.yaml
kubectl delete -f Infra/ngnix-controller.yaml
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

### Changing the local registry password
```bash
# Create a local file with the new 
docker run --rm httpd:2.4 htpasswd -Bbn <UserName> <Password> > htpasswd_file
# Create a docker config with with the new Pasword
#Then Encode in base64 the file
cat docker-config.json | base64 -w 0
#Copy the result in the in auth-secrets.yaml
```

---
### Run a curl container

```bash
docker run --rm curlimages/curl:8.11.1 sleep 3600
```

---

### For Local Development
```

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

### Add Migration
```bash
dotnet ef migrations add InitialMigration --project KubernetesExample.SharedDataStorage  --startup-project KubernetesExample.DbMigratorRunner -c AppDbContext -o Migrations
```