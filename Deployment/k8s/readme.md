Deploy
```
helm install coffeerecipes-api ./Deployment/k8s
```
Add Secrets
```
kubectl create secret generic api-secrets \
  --from-literal=ConnectionStrings__Marten="wartość_marten" \
  --from-literal=Media__ConnectionStrings="wartość_media"
```