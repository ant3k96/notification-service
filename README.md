# Notification Service 
This is .NET 8 application all dependencies are managed with the use of NuGet.
The service is capable of receiving data to be send either for email channel or sms channel or both at the same time. It supports two different providers in case one is unavailable during the request. 
If both providers are unavailable there is a retry mechanism implemented with the use of Polly. 



### Steps to launch
  1. Clone repository and open VS 2022 or similar IDE 
  2. Restore all dependancies
  3. In appsettings setup configuration to what suits your needs 
```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "SmsChannel": {
    "Enabled": false
  },
  "EmailChannel": {
    "Enabled": true
  },
  "Twillio": {
    "Enabled": true,
    "Priority": 2
  },
  "AmazonSns": {
    "Enabled": true,
    "Priority": 1
  }
}
```
  4. Launch solution and test using swagger or other method

