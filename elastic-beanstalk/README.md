## .Net Core version of the "Docker and Kubernetes: The Complete Guide" course.

> source code for the .Net Core version of the "Docker and Kubernetes: The Complete Guide" course.

## Execute it locally

$ docker-compose up --build

Navigate to http://localhost:3050/

## Continuous Integration with Travis CI + Amazon AWS

- The repository must be created on https://github.com/

- The repository must be assigned from GitHub on https://travis-ci.com/. The following setting variables must be set up:
1) AWS_ACCESS_KEY (for 11.-Multi-Container Deployments to AWS)
2) AWS_SECRET_KEY (for 11.-Multi-Container Deployments to AWS)
3) DOCKER_ID
4) DOCKER_PASSWORD

- The following instances must be created on Amazon (for 11.-Multi-Container Deployments to AWS)
1) Elastic Beanstalk (EB)
2) Relational Database Service (RDS) for Postgres
3) ElastiCache for Redis
4) Custom Security Group
5) Identity and Access Magagement (IAM)

## Within the code you can see how to
- Create different Docket Container Types and relate all of them
  1) React Client App
  2) .NET Core Web API
  3) .NET Core Console
  4) Postgres
  5) Redis
  6) NGINX
- Use Postgres from a Docker Container with .Net Core
- Use Redis from a Docker Container with .Net Core creating a subscription on the Web API App and subscribe to it on the Console App.
- Accept dynamic POST request with .Net Core API
- Send dynamic JSON responses from .Net Core API
- Use Docker Compose to run and relate easily different Docker Components
- Use NIGIX Container to run the React Client App
- Use NIGIX Container as Reverse Proxy with .NET Core API
- Work with different AWS Amazon service types to deploy a multi container Docker application using AWS Elastic Beanstalk
- Upload own Containers to Docker Hub and download them with the deployment
- Use Travis CI for the Continuous Integration Workflow
- Use Kubernetes
- Use Minikube to run Kubernetes locally
- Use Kubectl CLI for interacting with Kubernetes Master

## In order to get to know what has been developed follow the course on

https://www.udemy.com/docker-and-kubernetes-the-complete-guide

