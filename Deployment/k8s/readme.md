Deploy
```
helm install coffeerecipes-api ./Deployment/k8s
```
Add Secrets
```
kubectl create secret generic marten-secret --from-literal=connectionString="Host=ip;Port=6432;Database=coffeerecipes;Username=postgres;Password=password#;"
kubectl create secret generic media-secret --from-literal=connectionString="blobStorageConnectionString"
```