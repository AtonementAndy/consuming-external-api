# Consuming an External API with `IHttpClientFactory`

> **External API:** [JSONPlaceholder](https://jsonplaceholder.typicode.com/)
> **Stack:** .NET 8 (Console Application) · `Microsoft.Extensions.Hosting` · `Microsoft.Extensions.Http` · `xUnit` · `Moq`

---

## 📌 Overview

This repository demonstrates a **production-grade** approach to consuming an external HTTP API in .NET. 

The original version of this project created `HttpClient` instances manually (`new HttpClient()`), which — depending on request volume — exhausts the operating system's sockets and crashes the application. The adopted solution migrates to **`IHttpClientFactory`**, utilizing **Typed Clients**, Dependency Injection, and robust configuration management to ensure high performance, testability, and security.

---

## 🧨 The original problem: Socket Exhaustion

### What is it?

When you create a new `HttpClient` manually and dispose of it, the underlying TCP socket is **NOT released immediately**. It remains in the `TIME_WAIT` state for a period defined by the operating system (default: ~240s on Windows / ~60s on Linux).

In high-throughput scenarios (workers, APIs, loops), sockets accumulate until the available ephemeral port range is exhausted, resulting in `SocketException: Only one usage of each socket address is normally permitted`. This is the classic Socket Exhaustion.

### Why doesn't `using` solve it?

Because the problem is **not in the `HttpClient`** — it's in the internal handler (`HttpClientHandler` → `SocketsHttpHandler`). Recreating the `HttpClient` on every use also recreates the handler and, consequently, the socket.

---

## ✅ The solution: `IHttpClientFactory` + Typed Clients

Introduced in .NET Core 2.1, `IHttpClientFactory` solves the problem, and we've implemented it using the **Typed Client** pattern for maximum architectural cleanliness:

| Benefit | How it works |
| :--- | :--- |
| **Connection pool** | `HttpClientHandler` instances are reused internally |
| **Controlled lifecycle** | Handlers rotate every 2 minutes (recycling) |
| **Refreshed DNS** | Since the handler is recycled, IP changes are respected |
| **Centralized configuration** | Injected seamlessly via `appsettings.json` and `IOptions` |
| **Testability** | Easy to mock `HttpMessageHandler` for unit tests |
| **Separation of Concerns** | HTTP logic isolated inside a dedicated `UserService` |

> ⚠️ **Golden rule:** the `HttpClient` returned by the factory **is disposable**, but in the Typed Client pattern, the DI container manages its lifecycle. You must **never** call `_httpClient.Dispose()` manually inside your services.

---

## 🧱 Technologies and packages

| Package | Purpose |
| :--- | :--- |
| `Microsoft.Extensions.Hosting` | Generic host, DI, configuration, logging |
| `Microsoft.Extensions.Http` | `AddHttpClient(...)` and `IHttpClientFactory` |
| `Microsoft.Extensions.Options` | Dynamic configuration binding (`appsettings.json`) |
| `xUnit` & `Moq` | Unit testing and mocking framework |

---

## ⚙️ Pre-requisites

- .NET 8.0 SDK
- Git
- A terminal (PowerShell, Bash, zsh — any will do)

---

## 🚀 Step-by-step to run the project

1. **Clone the repository**
   Open your terminal and clone the project to your local machine.

2. **Restore dependencies**
   Run `dotnet restore` to install all the NuGet packages defined in the `.csproj` files.

3. **Run the application**
   Because this is a Solution with both the main app and a Test project, specify the project to run: `dotnet run --project CallWebApi`.

4. **Run the Unit Tests**
   Run `dotnet test` to execute the test suite (xUnit + Moq) and verify the service behaves correctly against mocked HTTP responses.

5. **Publish as an executable (Optional)**
   Run `dotnet publish -c Release -o ./publish` and execute the generated binary.

---

## 📁 Project structure

| Directory | Content |
| :--- | :--- |
| `CallWebApi/Configuration/` | `ApiSettings.cs` (Binds appsettings.json) |
| `CallWebApi/Models/` | `Address`, `Geo`, `Post`, `User` |
| `CallWebApi/Services/` | `IUserService`, `UserService` (Business logic) |
| `CallWebApi/` | `Program.cs`, `appsettings.json`, `CallWebApi.csproj` |
| `CallWebApi.Tests/` | `UserServiceTests.cs` (xUnit tests) |

---

## 🌐 API consumed
- **Base URL:** `https://jsonplaceholder.typicode.com/`
- **Endpoint:** `/users`
- **Format:** JSON
- **Authentication:** None (Public test API)

---

## 🎯 What was solved

| Before (`new HttpClient`) | After (Typed `IHttpClientFactory`) |
| :--- | :--- |
| Sockets exhausted under high load | Reused connection pool |
| DNS "frozen" at first resolution | Automatic handler recycling |
| Configuration hardcoded | Centralized in `appsettings.json` & `IOptions` |
| Hard to test | 100% testable via `Moq` and `HttpMessageHandler` |
| Spaghetti code in `Main` | Clean architecture with `IUserService` |

---

## 🛠️ Next steps (Future Improvements)

1. **Resilience:** Add `Microsoft.Extensions.Http.Polly` to implement Retry and Circuit Breaker policies.
2. **Caching:** Implement `IMemoryCache` or Redis for frequently requested data.
3. **Docker:** Containerize the application using a multi-stage `Dockerfile`.

---

Made with ❤️ by [AtonementAndy](https://github.com/AtonementAndy)
