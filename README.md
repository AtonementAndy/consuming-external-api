# Consuming an External API with `IHttpClientFactory`

> **External API:** [JSONPlaceholder](https://jsonplaceholder.typicode.com/)
> **Stack:** .NET 8 (Console Application) · `Microsoft.Extensions.Hosting` · `Microsoft.Extensions.Http`

---


# 📌 Overview

This console application demonstrates, in a **professional way**, how to consume an external HTTP API in .NET without falling into the classic **Socket Exhaustion** problem.

The original version of the project created `HttpClient` instances manually (`new HttpClient()`), which — depending on request volume — exhausts the operating system's sockets and crashes the application. The adopted solution was to migrate to **`IHttpClientFactory`**, which manages the `HttpClientHandler` lifecycle and the connection pool safely.

---


# 🧨 The original problem: Socket Exhaustion

### What is it?

When you do:

```csharp
using var client = new HttpClient();
```

The HttpClient is disposed at the end of the block, but the underlying TCP socket is NOT released immediately. It remains in the TIME_WAIT state for a period defined by the operating system (default: ~240s on Windows / ~60s on Linux).

In a scenario with:
- Many requests per second;
- Long-running services (workers, APIs, jobs);
- Loops calling the external API;

Sockets accumulate until the available ephemeral port range is exhausted. Result:

```bash
System.Net.Sockets.SocketException:
Only one usage of each socket address is normally permitted
```
This is Socket Exhaustion.

## Why doesn't `using` solve it?
Because the problem is not in the `HttpClient` — it's in the internal handler (`HttpClientHandler` → `SocketsHttpHandler`). Recreating the `HttpClient` on every use also recreates the handler and, consequently, the socket.


# ✅ The solution: `IHttpClientFactory`

Introduced in .NET Core 2.1, IHttpClientFactory solves the problem because:
| Benefit | How it works |
| :--- | :--- |
| **Connection pool** | `HttpClientHandler` instances are reused internally |
| **Controlled lifecycle** | Handlers rotate every 2 minutes (recycling) |
| **Refreshed DNS** | Since the handler is recycled, IP changes are respected |
| **Centralized configuration** | `BaseAddress`, headers, timeouts, etc. in one place |
| **Testability** | Easy to mock via DI |
| **Extensible** | Integrates with logging, custom handlers, etc. |


# 🧱 Technologies and packages
| Package | Purpose |
| :--- | :--- |
| `Microsoft.Extensions.Hosting` | Generic host, DI, configuration, logging |
| `Microsoft.Extensions.Http` | `AddHttpClient(...)` and `IHttpClientFactory` |
| `System.Net.Http.Json` | `GetFromJsonAsync`, `PostAsJsonAsync` extensions |

> `System.Net.Http.Json` ships with the .NET 5+ SDK — no installation needed.

# ⚙️ Pre-requisites
- [.NET 8.0](https://dotnet.microsoft.com/pt-br/download/dotnet/8.0)
- Git
- A terminal (PowerShell, Bash, zsh — any will do)

```bash
dotnet --version
# 8.0.x
```
# 🚀 Step-by-step to run the project

## 1. Clone the repository

```bash
git clone https://github.com/AtonementAndy/consuming-external-api.git
cd consuming-external-api/CallWebApi
```

## 2. Install the NuGet packages

```bash
dotnet add package Microsoft.Extensions.Hosting
dotnet add package Microsoft.Extensions.Http
```

## 3. Restore dependencies

```bash
dotnet restore
```

## 4. Run

```bash
dotnet run
```

## 5. (Optional) Publish as an executable

```bash
dotnet publish -c Release -o ./publish
./publish/CallWebApi
```


# 📁 Project structure
```text
CallWebApi/
├── Configuration/
│   └── ApiSettings.cs
├── Models/
│   ├── Address.cs
│   ├── Geo.cs
│   ├── Post.cs
│   └── User.cs
├── Services/
│   ├── IUserService.cs
│   └── UserService.cs
├── Program.cs
├── appsettings.json
└── CallWebApi.csproj
```


# 🌐 API consumed
- **Base URL**: `https://jsonplaceholder.typicode.com/`
- **Endpoint**: `/users`
- **Format:** `JSON`
- **Authentication**: `none (public test API)`


# 🎯 What was solved
| Before (`new HttpClient`) | After (`IHttpClientFactory`) |
| :--- | :--- |
| Sockets exhausted under high load | Reused connection pool |
| DNS "frozen" at first resolution | Automatic handler recycling |
| Configuration scattered across code | Centralized in `ConfigureServices` |
| Hard to test | Easy to mock via DI |
| No lifecycle management | Built-in handler rotation and disposal |


Made with ❤️ by [AtonementAndy](https://github.com/AtonementAndy)
