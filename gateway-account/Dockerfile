FROM mcr.microsoft.com/dotnet/aspnet:6.0
COPY bin/Release/net6.0/publish app/
WORKDIR /app
ENTRYPOINT ["dotnet", "gateway-account.dll"]

# dotnet publish -c Release
# sudo docker build -t l2/gateway-account:0.1.0 .
