# Notification Service 
This is .NET 8 application all dependencies are managed with the use of NuGet: 

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
