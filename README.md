# 📡 ALCI Order Stream | .NET 10 Real-Time SSE

A high-performance Order Tracking solution built with **.NET 10 Minimal APIs** and **Blazor WebAssembly**, featuring reactive updates via **Server-Sent Events (SSE)**. Optimized with `System.Threading.Channels` and **Tailwind CSS v4 Native DLL** for improved performance and enterprise-grade security.

## 🏗️ Architecture Overview

The solution follows a modular structure designed for high-speed development and containerized deployment:

* **Apps**:
    * `ALCI.OrderStream.API`: Reactive event hub with **Minimal APIs**.
    * `ALCI.OrderStream.Client`: Frontend **Blazor WASM** featuring a precision-crafted UI.
* **Core**:
    * `ALCI.OrderStream.Domain`: Shared business logic.
* **Tests**:
    * `ALCI.OrderStream.Api.Tests`: **Unit and integration** testing for the event pipeline using **xUnit**.
    * `ALCI.OrderStream.Client.Tests`: **UI component** testing for Blazor via **bUnit** and **xUnit**.
    * `ALCI.OrderStream.E2E.Tests`: **End-to-end (E2E)** testing with **Microsoft Playwright** and **xUnit**, validating the complete **SSE** lifecycle from API to Browser.
* **Tools**:
    * `Tailwind`: High-speed CSS compilation via specialized **TailwindCSS** binaries (**Windows & Linux**) for better performance and native-level execution. **Zero-Node.js** dependency workflow.
* **PowerShell Scripts**:
    * `start-alci.ps1`: Unified orchestrator for Local Development (**Windows/HTTPS**).
    * `start-alci-docker.ps1`: Full infrastructure orchestration (**Linux/Docker/HTTP**).

## ⚡ Key Features

* **.NET 10 SSE Resilience**: Custom protocol events designed to gracefully manage browser connections. For unidirectional streaming, this architecture outperforms **WebSockets** and **SignalR** by utilizing native **HTTP/2** multiplexing, reducing overhead and complexity while maintaining secure and persistent reactive updates.
* **Native Tailwind v4**: UI compiled via specialized binary for instant feedback: **no npm, no node_modules**.
* **Multi-Environment**: Intelligent `appsettings` hierarchy that automatically handles port mapping between Local Dev (**7038/7217**) and Docker (**8080/8081**).
* **Observability**: Structured logging and detailed **audit trail**.

## ⚙️ Prerequisites (Tailwind Native Setup)

To maintain a **Zero-Node.js** environment, this project requires the standalone Tailwind CLI binaries. Due to GitHub file size limits, you must download them manually:

1. Visit the [Tailwind CSS Releases](https://github.com/tailwindlabs/tailwindcss/releases).
2. Download the following binaries and place them in the `tools/tailwind/` folder:
    * **Windows**: `tailwindcss-windows-x64.exe` (required for Hot Reload).
    * **Linux**: `tailwindcss-linux-x64` (required for Docker/Linux builds).
3. **Important**: Ensure the filenames match exactly as listed above for the automation scripts to work.

## 🚀 Execution & Deployment

### Option A: Local Development (Windows)
Best for rapid UI/API iterations with **Hot Reload** and **HTTPS**.

```powershell
./start-alci.ps1
```

* **Client**: `https://localhost:7038`
* **API/Swagger**: `https://localhost:7217/swagger`

### Option B: Docker Infrastructure (Containerized)
Best for testing the production-ready environment (**Nginx + API**) in an isolated Linux ecosystem.

```powershell
./start-alci-docker.ps1
```

* **Client**: `http://localhost:8080`
* **API/Swagger**: `http://localhost:8081/swagger`

## 🧪 Running Tests

The solution uses **xUnit** as the unified standard across all testing layers.

### Unit & Component Tests
To run **API** and **UI** tests, execute from the solution root:
```powershell
dotnet test
```

### End-to-End (E2E) Tests
**E2E** tests require the infrastructure (Windows or Docker) to be **active**, as explained above, to validate real service communication.
To run **Playwright** tests and watch the automation in the browser:
```powershell
cd tests/ALCI.OrderStream.E2E.Tests
$env:HEADED="1"; dotnet test
```

## 🛠️ Testing the Collision

1. Open the **Client** URL and navigate to the tracking page for a specific `OrderId`.
2. Open **Swagger UI** and locate the **POST** `/orders/{orderId}/simulate` endpoint.
3. Execute the simulation. 
4. Watch the **Real-Time Stream** via **SSE** updating the web pages instantly.

## 🧹 Termination Protocol

When running locally via `start-alci.ps1`, the script ensures a clean exit by automatically terminating all related `dotnet` processes, preventing port-lock issues for your next session.

## 📝 Blog Post
I’ve written a detailed article explaining the "Why" and "How" of this architecture on my blog:  
🔗 **Beyond WebSockets: High-Performance Real-Time Streaming with .NET 10 Server-Sent Events (SSE)]([[https://andreecirillo.hashnode.dev/challeges-jsonprocessing-csharp](https://andreecirillo.hashnode.dev/beyond-websockets-high-performance-real-time-streaming-with-net-10-server-sent-events-sse)](https://andreecirillo.hashnode.dev/beyond-websockets-high-performance-real-time-streaming-with-net-10-server-sent-events-sse))**

---
**André Cirillo**
*(Architect of Light & Chaos | AI Developer & Full-Stack Fixer)*

**Powered by ALCI**
*(Artificial Large Collision Intelligence)*
