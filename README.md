# Resilience Notification Practice

This project is a **practice-oriented simulation** designed to demonstrate the implementation of **resilient communication patterns** in backend services using .NET and [Polly](https://github.com/App-vNext/Polly).

The goal is to simulate how notification systems (e.g., Email, SMS, Push) can remain **robust under failure**, using resilience techniques such as:

- **Retry** (in case of transient failures)
- **Timeout** (to avoid hanging operations)
- **Circuit Breaker** (to stop calling unstable services)
- **Fallback strategies** (alternative communication channels)

> 📌 **This project is not meant for production** – it's a focused training tool to understand key backend resilience concepts and fault-handling strategies.

---

##  Key Features

###  Resilience Strategies (via Polly)
- `Retry`: retries failed sends with exponential backoff
- `Timeout`: enforces maximum execution time per provider
- `Circuit Breaker`: temporarily disables unstable senders
- `Wrap`: combines all policies together for layered protection

###  Notification Dispatcher
- Dispatches notifications to multiple providers
- Supports **fallbacks** when primary channels fail
- Randomized failure simulation to trigger Polly behaviors

###  Multi-Provider Architecture
- Supports multiple channels:
  - Email
  - SMS
  - Push
- Each implements a common `ISender` interface

###  Clean Dependency Injection
- Providers are injected with their policies
- Policy is shared but abstracted for better testability

###  Structured Logging with [LoggerMessage]
- Logs retries, failures, fallbacks and successes
- Easily customizable and high-performance

---

## 🧪 How to Test

1. Run the project:
   ```bash
   dotnet run

2. Send a POST request using Postman or an .http file 
 ```json
     POST http://localhost:{port}/api/notification
    {
        "receiver": "hossein@example.com",
        "message": "Hello world!",
        "methods": [ 0, 1 ],
        "fallbacks": [ 2 ]
    }
```
3. Check for Logs

---
📦 Project Structure
```
resilience-notification-practice/
├── API/
│   └── NotificationEndpoints.cs
├── Core/
│   ├── Dispatchers/
│   ├── Interfaces/
│   │   └── ISender.cs
│   ├── Models/
│   │   ├── Enums/
│   │   └── NotificationRequest.cs
│   ├── Senders/
│   │   ├── Email/
│   │   ├── Sms/
│   │   └── Push/
│   └── Services/
│       └── NotificationSenderResolver.cs
├── DTOs/
│   ├── Requests/
│   └── Responses/
├── Infrastructure/
│   ├── Log/
│   │   └── NotificationLogTemplate.cs
│   └── Resilience/
│       └── NotificationPollyPolicies.cs
```
