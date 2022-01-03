FROM mcr.microsoft.com/dotnet/aspnet:6.0
COPY bin/Release/net6.0/publish app/
WORKDIR /app
ENTRYPOINT ["dotnet", "api-character.dll"]

# dotnet publish -c Release
# sudo docker build -t l2/api-character:0.1.0 .
