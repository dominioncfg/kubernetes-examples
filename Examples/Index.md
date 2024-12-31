# Kubernetes Quick Guide

## Get Api Resources

```bash
kubectl api-resources
```

---

## Pods

### List pods

```bash
kubectl get pods
```

### Describe Pod

```bash
kubectl describe pod <PodName>
```

### Remove Pod

```bash
kubectl delete pod <PodName>
```

### Get YAML of Pod

```bash
kubectl get pod <PodName> -o yaml
```

### Connect to a single pod container

```bash
kubectl exec -ti <PodName> -- <Command Ex, -- sh>
```

### Get Logs from a single conatiner pod
```bash
kubectl logs <PodName> [-f -> Live logs]
```

### Create Pod from YAML Manifest

```bash
kubectl apply -f pod.yaml
```

### Delete Pod from YAML Manifest

```bash
kubectl delete -f pod.yaml
```

### Run Pod from image

```
kubectl run mynginx --image=nginx --labels="app=hazelcast,env=prod"
```

---

## Replica Sets

Replica sets use labels to match Pods, even if they already exist.

### Get List of Replica Sets

```bash
kubectl get replicaset
```

### Describe Replicaset

```bash
 kubectl describe replicaset frontend  <Name>
```

---

## Deployments

Deployments resource manage Replica Sets.

### Get Status of Deployments

```bash
kubectl rollout status deployment <DeploymentName>
```

### Lists Deployments

```bash
kubectl get deployment
```

### Get YAML of a Deployment

```bash
kubectl get deployment <DeploymentName> -o yaml
```

### Describe Deployment

```bash
kubectl describe deployment <DeploymentName>
```

### Apply and Update Deployment
```bash
kubectl apply -f <DeploymentFileName.yaml>
```

### See History of Deployment
```bash
kubectl rollout history deployment <DeploymentName>
```

### Rollback Deployment
```bash
kubectl rollout undo deployment <DeploymentName> --to-revision=<RevisionNumber>
```

---

## Services

Services watch Pods with given labels and distribute the load among them. They have a static IP and create endpoints for Pods.

### Types of Services:
1. **ClusterIP**: Internal IP within the cluster (default).
2. **NodePort**: Exposes a port externally on each Node.
3. **LoadBalancer**: Cloud-based; creates a load balancer resource.

### Commands

#### Create a Service

```bash
kubectl apply -f service.yaml
```

#### Get Services

```bash
kubectl get services [-l app=front]
```

#### Describe Service

```bash
kubectl describe service <ServiceName>
```

#### Describe Endpoints of a Service

```bash
kubectl describe endpoints <ServiceName>
```

---

## Namespaces

### DNS for Namespace
`<ServiceName>.<Namespace>.svc.cluster.local`

### List Namespaces

```bash
kubectl get namespaces
```

### Create Namespaces

```bash
kubectl create namespace <Name>
```

### Get Labels of Namespaces

```bash
kubectl get namespaces --show-labels
```

### Describe a Namespace

```bash
kubectl describe namespaces <Name>
```

### Create Namespace using YAML

```bash
kubectl apply -f namespace.yaml
```

### Get Pods in Namespace

```bash
kubectl get pods -n <Name>
```

#### Delete in Namespace

```bash
kubectl delete pod <PodName> -n <Name>
```

### Apply in Namespace

```bash
kubectl apply -f <FileName>.yaml -n <Name>
kubectl apply -f pods.yaml -n development
```

### Create a Context in Kubernetes with Default Namespace

```bash
kubectl config current-context
kubectl config view
kubectl set-context <Name>  --namespace=<NamespaceName> --cluster=<ClusterName> --user=<UserName>
kubectl config use-context <ContextName>
```

```bash
kubectl config set-context minikube-development  --namespace=development --cluster=minikube --user=minikube
kubectl config use-context minikube-development
```

---

## Resource Limits and Requests

### Apply a Limit of CPU and RAM to a Pod

Request maximum can be exceeded up to the limit as long as resources are available in Kubernetes.

---

## Limit Range

Apply requests and limits to individual objects in a namespace.

### List Limit Ranges

```bash
kubectl get limitrange
```

### Describe Limit Range

```bash
kubectl describe limitrange <Name> -n <Namespace>
```

### Limits Can Be Seen by Describing the Namespace

```bash
kubectl describe namespace default
```

---

## Resource Quota

Limit the overall resources consumed by a namespace.

### Apply a Resource Quota from YAML
```bash
kubectl apply -f resourcequota.yaml
```

### Get All Resource Quotas

```bash
kubectl get resourcequotas
```

### Describe Resource Quota
```bash
kubectl describe resource quota
```

### Quotas Can Be Shown by Describing the Namespace
```bash
kubectl describe namespace <NamespaceName>
```

---

## Probes

- **Liveness Probe**: Checks if a container is alive at any given time.
- **Readiness Probe**: Checks if a pod is ready to receive traffic.
- **Startup Probe**: For applications that take too long to start.

---

## Config Maps

### Create a Config Map from a File

```bash
kubectl create configmap <Name> --from-file=<FilePath>
```

### List Config Maps

```bash
kubectl get configmaps
```

### Describe Config Map

```bash
kubectl describe configmap <Name>

```

---

## Secrets

### Create a Secret from a File

```bash
kubectl create secret generic <Name> --from-file=<FilePath>
```

### List Secrets

```bash
kubectl get secrets
```

### Describe Secret

```bash
kubectl describe secret <Name>
```

### Delete Secret

```bash
kubectl delete secret my-secret
```

---

## Volumes

### Volume Types:
- **EmptyDir**: Shared folder by containers in the pod; data is lost if the pod dies.
- **HostPath**: A folder in the node where the pod runs; data is not retained if the pod restarts in a different node.
- **Cloud Volume**: For workloads running in the cloud.
- **Persistent Volume (PV) and Persistent Volume Claim (PVC)**: Typically used in cloud environments.

### Reclaim Policies:
- **Retain**: Data is not lost when the PV is removed.
- **Recycle**: Data is lost but the PV is not deleted.
- **Delete**: Data and the PV are both removed.

### List Persistent Volumes

```bash
kubectl get persistentvolumes
```

### Get Persistent Volume Claims

```bash
kubectl get persistentvolumeclaims
```

### Get Storage Classes

```bash
kubectl get storageclass
```

---

## Role Based Access Control (RBAC)

### Roles vs Cluster Roles
- **Roles** are limited to a namespace, while **Cluster Roles** are for the entire cluster.

### Certificate Signing Request

The Common Name (CN) becomes the Kubernetes Username, and the Organization becomes the Group.

### How to Create a User in Kubernetes

#### Create and Sign a Certificate for the User
1. Generate a private key:

    ```bash
        openssl genrsa -out <CertificateName>.key 2048
    ```
2. Create a signing request:
    ```bash
    openssl req -new -key <CertificateName>.key -out <Signed>.csr -subj "/CN=<UserName>/O=<Group>"
    ```
3. Sign the certificate:
    ```bash
    openssl x509 -req -in <Signed>.csr -CA <PathToKubernetesCA>.crt -CAkey <PathToKubernetesCAKey>.key -CAcreateserial -out <Certificate>.crt -days 500
    ```
4. Check the certificate:

    ```bash
    openssl x509 -in <SignedCertificate>.crt -noout -text
    ```

#### Example
```bash
openssl genrsa -out pedrito.key 2048
openssl req -new -key pedrito.key -out pedrito.csr -subj "/CN=pedrito/O=developer" => this one doesnt work on git bash
openssl x509 -req -in pedrito.csr -CA "C:\\Users\\josec\\.minikube\ca.crt" -CAkey "C:\\Users\\josec\\.minikube\ca.key" -CAcreateserial -out pedrito.crt -days 500 
openssl x509 -in pedrito.crt -noout -text
```
---

#### Configure Kubectl to use the signed certificate
1. Set the Credentials of User to Kubernetes Config
   ```bash
   kubectl config set-credentials your-username --client-certificate=/path/to/user.crt --client-key=/path/to/user.key --embed-certs=true
    ```
2. Add A new Context:

   ```
   kubectl config set-context your-context --cluster=your-cluster --namespace=default --user=your-username
   ```

3. Switch to the context:
  
   ```
   kubectl config use-context your-context
   ```

#### Example
```bash
kubectl config set-credentials pedrito --client-certificate="C:\\Users\\josec\\source\\repos\\KubernetesExample\\Examples\\10.Users\\pedrito.crt" --client-key="C:\\Users\\josec\\source\\repos\\KubernetesExample\\Examples\\10.Users\\pedrito.key" --embed-certs=true
kubectl config set-context pedri --cluster=minikube --namespace=default --user=pedrito
kubectl config use-context pedri
```

### Roles

#### List Roles

```bash
kubectl get roles
```

#### Describe Role

```bash
kubectl describe role
```

---

### Roles Binding

#### Get Roles Bindings

```bash
kubectl get rolebindings
```

#### Describe Role Binding
```bash
kubectl describe rolebinding
```

---

### Cluster Roles Commands:

#### List Cluster Roles
```bash
kubectl get clusterroles
```

#### Describe Cluster Role
```bash
kubectl describe clusterrole system:controller:ttl-controller
```

---

### Cluster Roles Binding:

#### List Role Bindings

```bash
kubectl get rolebindings
```

#### Describe Role Binding

```bash
kubectl describe clusterrolebinding
```

---


## Service Accounts

Every namespace has a service account and each of them are associated to a secret.

### List Service Account
```bash
kubectl get serviceaccounts
```

### Describe Service Account
```bash
kubectl describe serviceaccount default
```

---

## Jobs

### List Jobs

```bash
kubectl get jobs
```

### Describe a Job

```bash
kubectl describe job example-job
```

### View Pods of a Job

```bash
kubectl get pods --selector=job-name=example-job
```

### List CronJobs
```bash
kubectl get cronjobs
```

### Describe a CronJob
```bash
kubectl describe cronjob example-cronjob
```

---

## DaemonSets

### List

```bash
kubectl get daemonsets
```

---

## StatefulSets

Pods get an incremental ID; they are created in ascending order, waiting for the previous one to be fully created. Deletion works in the same way but in descending order. Each Pod, even after restart, will get the exact same state (volumes) attached.

### What is a StatefulSet in Kubernetes?

A StatefulSet is a Kubernetes resource used to manage stateful applications that require unique, persistent identities and stable storage. Unlike Deployments or ReplicaSets, StatefulSets ensure that each Pod:
- Has a unique identity (e.g., pod-0, pod-1).
- Retains its identity across restarts.
- Maintains a stable hostname (DNS).
- Can be associated with persistent storage, ensuring data is not lost when a Pod is rescheduled.

### Key Characteristics of StatefulSets:
- **Stable Pod Names**: Pods are created in order and have predictable names like `<statefulset-name>-<ordinal>`.
- **Ordered Deployment and Scaling**: Pods are created, updated, and deleted sequentially.
- **Persistent Storage**: Each Pod can have its own PersistentVolumeClaim (PVC) for storage.

### Use Cases:
- Databases (e.g., MySQL, MongoDB, Cassandra).
- Distributed systems (e.g., Kafka, Zookeeper).
- Stateful applications that need consistent DNS names or persistent data.

### List Staful Sets:

```bash
kubectl get statefulsets
```

