# Create the Docker Images
docker build -t peelmicro/dotnet-core-multi-client:latest -t peelmicro/dotnet-core-multi-client:$SHA -f ./client/Dockerfile ./client
docker build -t peelmicro/dotnet-core-multi-server:latest -t peelmicro/dotnet-core-multi-server:$SHA -f ./server/Dockerfile ./server
docker build -t peelmicro/dotnet-core-multi-worker:latest -t peelmicro/dotnet-core-multi-worker:$SHA -f ./worker/Dockerfile ./worker

# Take those images and push them to docker hub
docker push peelmicro/dotnet-core-multi-client:latest
docker push peelmicro/dotnet-core-multi-client:$SHA
docker push peelmicro/dotnet-core-multi-server:latest
docker push peelmicro/dotnet-core-multi-server:$SHA
docker push peelmicro/dotnet-core-multi-worker:latest
docker push peelmicro/dotnet-core-multi-worker:$SHA
# Apply all configs in the 'k8s' folder
kubectl apply -f k8s
# Imperatively set lastest images on each deployment
kubectl set image deployments/client-deployment client=peelmicro/dotnet-core-multi-client:$SHA
kubectl set image deployments/server-deployment server=peelmicro/dotnet-core-multi-server:$SHA
kubectl set image deployments/worker-deployment worker=peelmicro/dotnet-core-multi-worker:$SHA