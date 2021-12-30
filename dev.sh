#!/bin/bash
sudo docker-compose down
sudo docker rmi l2/api-account:0.1.0
sudo docker rmi l2/gateway-account:0.1.0
cd ./api-account/
sudo rm -rf ./bin/
sudo rm -rf ./obj/
dotnet publish -c Release
sudo docker build -t l2/api-account:0.1.0 .
cd ../gateway-account/
sudo rm -rf ./bin/
sudo rm -rf ./obj
dotnet publish -c Release
sudo docker build -t l2/gateway-account:0.1.0 .
cd ../
sudo docker-compose up -d
