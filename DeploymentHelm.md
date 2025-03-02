# Deployment Guide

## Introduction

This guide provides a step-by-step walkthrough for deploying a full-stack application to a Kubernetes cluster. The application is composed of the following components:

1. **PostgreSql Database:** The persistent data storage solution. Using a Master Replica DB Cluster
2. **.NET Core Web API:** A backend service that handles business logic and interacts with the database.
3. **Database Migration Job:** A utility for deploying and applying database schema migrations.
4. **Nginx Frontend:** A web server hosting the client-facing interface.

The deployment process leverages Kubernetes features such as pods, services, and ingress controllers to orchestrate and expose the application. By following this guide, you'll learn how to:

* Build and containerize each application component.
* Configure and deploy Kubernetes resources for a seamless deployment.
* Apply database migrations to ensure the database schema is ready for the backend API.
* Set up an Nginx ingress controller to manage routing and load balancing.

The guide is tested with minikube in mind and is using Helm Templates. 

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
minikube ssh 
curl -u jc:Patata@1 http://host.docker.internal:5000/v2/_catalog
```

---
## Deploy the Database
Database consist in a Master-Replica of PostgreSql.

### Apply Master Database Stateful Set
```bash
helm install students-db Infra/helm/database

```

## Apply Database Migrations

### Build Database Migrator Image Image
```bash
docker build -t localhost:5000/students-db-migrator:v1.4 -f KubernetesExample.DbMigratorRunner/Dockerfile .
```


### Push The Image to the local repository
```bash
docker push localhost:5000/students-db-migrator:v1.4
```

### Deploy Migrations Job
```bash
helm install students-db-migrator Infra/helm/database-migrator
```

---

## Deploy the Backend

### Build Backend Image
```bash
docker build -t localhost:5000/students-api:v1.4 -f KubernetesExample/Dockerfile .
```

###  Push The Image to the local repository
```bash
docker push localhost:5000/students-api:v1.4
```

### Install Backend Deployment
```bash
helm install students-backend Infra/helm/backend
```

---

## Deploy the Frontend

### Build Frontend Image
```bash
docker build -t localhost:5000/frontend-server:v1.4 -f Infra/Html/Dockerfile Infra/html
```

### Push The Image to the local repository
```bash
docker push localhost:5000/frontend-server:v1.4
```

### Install Frontend Deployment
```bash
helm install students-frontend Infra/helm/frontend
```

---

## Deploy Ingress Routes

### Setup and Nginx Ingress Controller
```bash
kubectl apply -f Infra/ngnix-controller.yaml
```
### Deploy the Routes
```bash
helm install students-app-rules Infra/helm/ingress-configuration-routes
```

### Expose the Nginx Ingress (Only requireded in Minikube)
```bash
minikube service ingress-nginx-controller --url  -n ingress-nginx
```


### Open the frontend App
---
The App should be listening in `http://127.0.0.1:XXXXX/app`

## Clean up all resources after you finish

### Delete all the deployed resources
```bash
helm uninstall students-app-rules 
helm uninstall students-frontend
helm uninstall students-backend 
helm uninstall students-db-migrator
helm uninstall students-db 
kubectl delete -f Infra/Local/auth-secrets.yaml
kubectl delete -f Infra/ngnix-controller.yaml
docker-compose --file Infra/Local/docker-compose.yml down
```

### Delete the Database Persistent Volume and its Claim
```bash
kubectl delete pvc archive-data-students-db-students-database-db-rset-0  
kubectl delete pvc postgres-data-students-db-students-database-db-rset-0
```

### Delete PV if they were nore removed properly
```bash
kubectl get pv
#If any PV exist do: 
kubectl delete pv <Name>
```

---
## Other scripts

### Connect to the datbase

#### Deploy the included debugging pods
There are two pods included one for ssh and another one with a PlSqlClient
```bash
kubectl apply -f Infra/debug-container.yaml
```

#### Connect to the SQL Client Pod
```bash
kubectl exec -it psql-client -- sh 
```

#### Connect to the Database
```bash
psql -h students-db-students-database-db-rset-0.students-db-students-database-db-svc -U studentApiAdmin -d studentsdb
```

#### Run a Query
```sql
#Get tables in the std schema
\dt std.*
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

### Run Pg Server Image Locally
```bash
docker run --name pg-sql -e POSTGRES_PASSWORD=PataDeCabra@2020 -p 5432:5432 -d postgres:15.0
```

### Test Locally Docker Image
```bash
docker run --network host -d students-api:v1.3
```

### Add Migration
```bash
dotnet ef migrations add InitialMigration --project KubernetesExample.SharedDataStorage  --startup-project KubernetesExample.DbMigratorRunner -c AppDbContext -o Migrations
```

