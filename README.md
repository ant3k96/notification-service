# Notification Microservice

## Overview

The **Notification Microservice** is a .NET 8-based application designed to handle email and SMS notifications using multiple providers. The service supports **Twilio** and **Amazon SNS** (mocked services) as providers for both channels. It uses **MediatR** for message dispatching and **Polly** as a retry mechanism to enhance reliability.

## Features

- **Two Notification Channels**: Email and SMS.
- **Multiple Providers**:
  - Email: Twilio SendGrid, Amazon SNS (mocked services)
  - SMS: Twilio, Amazon SNS (mocked services)
- **MediatR for Message Handling**
- **Polly for Retry Mechanism**
- **REST API Endpoint** to send notifications
- **Swagger UI** for API exploration and testing

## Tech Stack

- **.NET 8**
- **MediatR** (CQRS pattern)
- **Polly** (Resilience & retry mechanism)
- **FluentValidation** (Request validation)
- **Swagger (NSwag)** (API documentation)
- **Dependency Injection** (Built-in .NET DI container)

## Installation & Setup

### Prerequisites

- .NET 8 SDK
- Visual Studio 2022 / Rider / VS Code
- Docker CLI

### Clone the Repository

```sh
git clone https://github.com/ant3k96/notification-service.git
cd notification-microservice
```

### Configuration

The service requires app settings for the providers. Modify the `appsettings.json` or use environment variables.

#### Example `appsettings.json`

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
  "TwillioSms": {
    "Enabled": true,
    "Priority": 2
  },
  "TwilioSendGridEmail": {
    "Enabled": true,
    "Priority": 2
  },
  "AmazonSnsSms": {
    "Enabled": true,
    "Priority": 1
  },
  "AmazonSnsEmail": {
    "Enabled": true,
    "Priority": 1
  }
}
```

### Run the Application

#### Using .NET CLI

```sh
dotnet build
dotnet run
```

#### Using Docker

```sh
cd NotificationService 

docker build -t notification-service -f NotificationApi/Dockerfile .
docker run -p 5000:8080 notification-service
```

## API Documentation (Swagger)

Once the application is running, you can access the Swagger UI at:

```
http://localhost:7000/swagger
```

This allows you to test the notification API endpoints.

## API Endpoints

### **Send Notification**

- **Endpoint:** `POST /api/send_message`
- **Request Body:**

```json
{
  "sms": {
    "to": "string",
    "from": "string"
  },
  "email": {
    "emailTo": "string",
    "emailFrom": "string",
    "subject": "string"
  },
  "body": "string"
}
```

- **Response:**

200 OK or Error message

## Architecture

### **MediatR and Polly Implementation**

- `INotificationHandler<SendMessageRequest>` is used to handle message processing.
- The Polly decorator (`RetryDecorator<T>`) ensures retry logic is applied to failed notification requests.

### **Service Registration**

Services are conditionally registered based on configuration settings:

```csharp
if (amazonSnsEmailOptions.Enabled)
    services.AddSingleton<IEmailProvider, AmazonSnsEmailMockService>();

if (twilioSmsOptions.Enabled)
    services.AddSingleton<ISmsProvider, TwilioSmsMockService>();
```
---

