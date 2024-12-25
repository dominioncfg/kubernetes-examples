Run Sql Server Image:
docker run -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=PataDeCabra@2020" -p 1433:1433 -d mcr.microsoft.com/mssql/server:2022-latest 

Build Backend Image
docker build -t students-api:v1.1 -f KubernetesExample/Dockerfile .

Test Locally Docker Image
docker run --network host -d students-api:v1

Load Backend Image to Minikube Cluster
minikube image load students-api:v1

Run MSSQL Tools in the cluster to test db 
kubectl run -i --tty --rm debug --image=mcr.microsoft.com/mssql-tools:latest --restart=Never

sqlcmd -S data-base-service -U sa -P PataDeCabra@2020
SELECT name, database_id, create_date FROM sys.databases; 
GO

Build Frontend Image
docker build -t frontend-server:v1.0 -f Dockerfile .

Load Frontend Image into the Minkube Cluster
minikube image load frontend-server:v1.0

Expsoe Backend Service outside the cluster
minikube service backend-service --url

Expose Frontend Service outside the cluster
minikube service front-end-service --url