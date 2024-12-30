--------Useful
Get Api Resouces
kubectl api-resources

-------1. Pods
1. Get List of pods
kubectl get pods

2. Describe Pod
kubectl describe pod <PodName>

3. Remove Pod
kubectl delete pod <PodName>

5. Get Yaml of Pod
kubectl get pod <PodName> -o yaml

6- Connect to container 
Kubectl exec -ti <PodName> -- <Command Ex, -- sh> 

6. Get Logs Pod
kubectl logs <PodName> <-f -> Live logs>

4. Create From Yaml Manifest
kubectl apply -f pod.yaml 

5. Delete from Yaml Mannifest
kubectl delete -f pod.yaml 

Others:
Run Pod
kubectl run mynginx --image=nginx --labels="app=hazelcast,env=prod"

-------2. Replica Sets
Replica set use labes to match pods even if they were already existing.

1. Create Replica Sets from manifest 
kubectl apply -f rs.yaml 

2. Get List of Replicaset
kubectl get replicaset


-----3. Deployments
Deployment Manage Replica Sets
1. Get Status of Deployments
kubectl rollout status deployment <DeploymentName>

2. Get List of Deployments
kubectl get deployment

3. Describe Deployments
kubectl get deployment  <DeploymentName> -o yaml

4. Describe Deployments
kubectl describe deployment  <DeploymentName>


5. Apply and Update Deployment
kubectl apply -f <DeploymentFileName.yaml>


6. See history of deployment
kubectl rollout history deployment <DeploymentName>

7. Use Annotations

8. Get Revision Changes
kubectl rollout history deployment <DeploymentName> --revision=<RevistionNumber>


9. Rollback
kubectl rollout undo deployment <DeploymentName> --to-revision=<RevisitonNumber>


------Services
Services watch pods with given labels and distributes the load between them it has a static ip
Services create a endpoint behind the scenes and register pods in that endpoint
Service use the service name as dns (needs to be unique between dns)
Services Types
    1. Cluster IP: Internal IP in the cluster, is constant over time.
    2. NodePort: Expose a port in the Node externally (Behind the scenes it creates a ClusterIP + Opens the port to the external traffic)
    3. LoadBalancer: Only works for cloud, behind the scenes it creates a load balancer resource in the cloud and register a node port in that resource

1. Create a Service


2. Get Services
kubectl get services [-l app=front]

3. Describe Service
kubectl describe service <ServiceName>

4. Describe endpoints of a service
kubectl describe endpoints <ServiceName>


------Namespaces

1. Get All namespaces
kubectl get namespaces

2. Create Namespaces
kubectl create namespace <Name>

3. Get Labels 
kubectl get namespaces --show-labels

4. Describe
Kubectl describe namespaces <Name>

5. Create Namespace using yaml

6. Get pods in namespace
kubectl get pods -n <Name>
-> Delete in namespace:
kubectl delete pod <PodName> -n <Name>

7. Apply In namespace
kubectl apply -f <FileName>.yaml -n <Name>
kubectl apply -f pods.yaml -n development

8. Deploy to namespace


9.DNS for namespace:
Access DSN for Services in different namespaces: <ServiceName>.<Namespace>.svc.cluster.local

10. Create a context in kubernetes with defaul namespace
kubectl config current-context
kubectl config view
kubectl set-context <Name>  --namespace=<NamespaceName> --cluster=<ClusterName> --user=<UserName>
kubectl config use-context <ContextName>

kubectl config set-context minikube-development  --namespace=development --cluster=minikube --user=minikube
kubectl config use-context minikube-development

-------Resource Limits and Requests
1. Apply a limit of cpu and ram to a pod
Request maximun can be exceded up to the Limit max as long as the are available resources in the Kubernetes 

-------Limits Range
Apply request and limits to individual objects in namespace.

1. APply a limit range from yaml

2. Get all limit range
kubectl get limitrange

3. Describe 
kubectl describe limitrange <Name> -n <Namespace>

4. Limit can be seen by describing the namespace
kubectl describe namespace default



-------Resource Quota
Limit the overall resources consumed by a namespace

1. Apply a resource quota from yaml

2. Get all resource quotas:
kubectl get resourcequotas 

3.Describe Resource Quota
kubectl describe resource quota

2. Quotas can also be shown by describing the namespace 
kubectl describe namespace <NamespaceName>

----Probes:
Liveness -> Proves to know if a containers is alive at any given point in time
Readiness-> Pod is ready to recieve traffic
Startup-> For application that take to long to start
----Config Maps

1. Create a config map from a file
kubectl create configmap <Name> --from-file=<FilePath>

2. Get All config Maps:
kubectl get configmaps

3. Describe
kubectl describe configmap <Name>


-------Secrets

1. Create a secret from a file
kubectl create secret generic <Name> -- from-file=<FilePath>

2. Get Secrets:
kubectl get secrets

3. Describe Secret
kubectl describe secret <Name>

4. Delete Secret:
kubectl delete secret my-secret


-----------Volumes
Types:
    EmptyDir: A folder shared by container in the logs, if the pod dies the data is lost
    HostPath: A folder in the node where the pod is running, if the pod dies and it recreated in another node, the data wont be there
    Cloud Volume: For workloads running in the cloud (Not really used)
    Persistence Volume and Persistence Volume Claim: NOrmally used in cloud
Reclaim Policies
    Retain: When a PVV is removed Data is not lost
    Recycle: Data is lost but PV is not delete
    Delete: Data and PV are also removed

1. Get Persistence volumes
kubectl get persistentvolumes

2. Get Persistence Volume Claims
kubectl get persistentvolumeclaims

3.Get Storage Class:
kubectl get storageclass


-----RBAC
Role vs Cluster Role: Roles are limited to a namespace, while Cluster role are for the whole cluste r  
Certificate Signging Request
    Common Name (CN) becomes the Kubernetes UserName
    Organization becomes the Group 

How to Create an user in Kubernetes:

 1. Create and sign a certificate for the user:
    Create and Sign Certificate
    1.1. Create certificate and Signing Request
    openssl genrsa -out <CertificateName>.key 2048
    1.2. Create a signing Request
    openssql req -new -key <CertificateName>.key -out <Signed>.csr - subj "/CN=<UserName>/O=<Group>"
    1.3. Sign the certificate (With Kubernetes Certificate)
    openssl x509 -req -in <Signed>.csr -CA <PathToKubernetsCA>.crt - CAkey <PathToKubernetsCAKey>.key - CAcreateserial -out <Certificate>.crt - days 500 
    1.4. Check the certificate:
    openssl x509 -in <SignedCertificate>.crt -noout -text
Example:
openssl genrsa -out pedrito.key 2048
openssl req -new -key pedrito.key -out pedrito.csr -subj "/CN=pedrito/O=developer" => this one doesnt work on git bash
openssl x509 -req -in pedrito.csr -CA "C:\\Users\\josec\\.minikube\ca.crt" -CAkey "C:\\Users\\josec\\.minikube\ca.key" -CAcreateserial -out pedrito.crt -days 500 
openssl x509 -in pedrito.crt -noout -text

4.Configure Kubectl to use the signed certificate
   4.1 Set the Credentials of User to Kubernetes Config:
   kubectl config set-credentials your-username --client-certificate=/path/to/user.crt --client-key=/path/to/user.key --embed-certs=true

   4.2 Add A new Context:
   kubectl config set-context your-context --cluster=your-cluster --namespace=default --user=your-username

   4.3 Switch to the context:
   kubectl config use-context your-context

kubectl config set-credentials pedrito --client-certificate="C:\\Users\\josec\\source\\repos\\KubernetesExample\\Examples\\10.Users\\pedrito.crt" --client-key="C:\\Users\\josec\\source\\repos\\KubernetesExample\\Examples\\10.Users\\pedrito.key" --embed-certs=true
kubectl config set-context pedri --cluster=minikube --namespace=default --user=pedrito
kubectl config use-context pedri


Roles Commands: 
1. Get Roles
kubectl get roles
2. Describe Role
kubectl describe role <Name>

Roles Binding Commands: 
1. Get Roles Bindings
$ kubectl get rolebindings
2. Describe Role
kubectl describe rolebinding <Name>


Cluster Roles Commands: 
1. Get Roles
kubectl get clusterroles
2. Describe Role
kubectl describe clusterrole system:controller:ttl-controller <Name>

Cluster Roles Binding Commands: 
1. Get Role Binding
$ kubectl get rolebindings
2. Describe Role
kubectl describe clusterrolebinding <Name>

--Apply Roles as groups

----Service Accounts
Every namespace has a service account and each of them are associated to a secret.

1. List Service Account
kubectl get serviceaccounts

2. Describe 
kubectl describe serviceaccount default


--------JOBS------

1. 
kubectl get job

Create Jonbs
   -Number of Completions and Parallelism
   -Number of Failed Before is considered Failed
   -RestartPolicy
   -Deadline
   -Create a cron  Job


2. Deamon Sets
A deployment that runs a copy of a service on each node


Steful Sets:
1. Pods get an incremental id, they are created in ascending order waiting for the previous one to be fully created.
Deletion works in the same way but descending order, each pod even after restart they will get the exact same state (volumes) attached


