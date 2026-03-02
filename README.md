# Kubernetes Example

A hands-on reference project for learning and experimenting with Kubernetes. It combines a deployable .NET application stack with annotated YAML examples covering a wide range of Kubernetes concepts.

## Projects

| Project | Description |
|---|---|
| `KubernetesExample` | .NET Core Web API (backend) |
| `KubernetesExample.BFF` | .NET Core Web API acting as a Backend For Frontend |
| `KubernetesExample.DbMigratorRunner` | Database migration job |
| `KubernetesExample.SharedDataStorage` | Shared data layer |

The frontend is a static site served via Nginx.
The database is PostgreSQL, configured as a master/replica cluster.

## Repository Structure

```
Examples/           # Numbered YAML examples organized by Kubernetes concept
Helm/               # Helm charts for deploying the application stack
Infra/              # Kubernetes manifests for deploying the full app
Others/             # Helpers (VM setup, Postgres replication locally)
Deployment.md       # Step-by-step deployment guide (minikube)
DeploymentHelm.md   # Helm-based deployment guide
```

## Kubernetes Concepts Covered

The `Examples/` folder walks through topics in order:

1. Pods
2. ReplicaSets
3. Deployments
4. Services
5. Namespaces
6. Resource Limits & Quotas
7. ConfigMaps
8. Secrets
9. Volumes
10. Users & RBAC
11. Groups
12. Service Accounts
13. Ingress
14. Jobs & CronJobs
15. DaemonSets
16. StatefulSets

Advanced topics include **Istio** (canary releases, ingress, fault injection, circuit breaker) and **Helm** (charts, subcharts, templating).

## Getting Started

See [Deployment.md](Deployment.md) for a full walkthrough using minikube, or [DeploymentHelm.md](DeploymentHelm.md) for the Helm-based approach.

A quick-reference command guide is available in [Examples/Index.md](Examples/Index.md).
