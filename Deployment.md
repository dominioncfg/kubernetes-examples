1. ------------Database------------

1.1 Apply Database Deployment
kubectl apply -f Infra/database.yaml

2. ------------Backend------------ 
2.1 Build Backend Image
docker build -t students-api:v1.3 -f KubernetesExample/Dockerfile .

2.2 Load Backend Image to Minikube Cluster
minikube image load students-api:v1.3

2.3 Apply Backend Deployment
kubectl apply -f Infra/backend.yaml

3. ------------Front End------------

3.1 Build Frontend Image
docker build -t frontend-server:v1.3 -f Infra/Html/Dockerfile Infra/html

3.2 Load Frontend Image into the Minkube Cluster
minikube image load frontend-server:v1.3

3.3 Apply Frontend Deployment
kubectl apply -f Infra/frontend.yaml

------------

4. ------------Ingress------------

4.1 Setup and Ngnix Ingress Controller (Examples/Ingress)

4.2 Deploy the Routes
kubectl apply -f Infra/ingress-routes.yaml


------------
Delete all Resources  from the cluster

kubectl delete -f Infra/database.yaml
kubectl delete -f Infra/backend.yaml
kubectl delete -f Infra/frontend.yaml
kubectl delete -f Infra/ingress-routes.yaml
------------- 

1. Run a pod inside the cluster and connect to the database

Run MSSQL Tools in the cluster to test db 
kubectl run -i --tty --rm debug --image=mcr.microsoft.com/mssql-tools:latest --restart=Never

sqlcmd -S data-base-service -U sa -P PataDeCabra@2020
SELECT name, database_id, create_date FROM sys.databases; 
GO



2. For Local Development:

Run Sql Server Image:
docker run -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=PataDeCabra@2020" -p 1433:1433 -d mcr.microsoft.com/mssql/server:2022-latest 


Test Locally Docker Image
docker run --network host -d students-api:v1.2