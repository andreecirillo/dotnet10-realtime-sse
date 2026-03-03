# 📡 ALCI Order Stream | .NET 10 Real-Time SSE

Uma solução de rastreamento de pedidos de alta performance construída com **.NET 10 Minimal APIs** e **Blazor WebAssembly**, apresentando atualizações reativas via **Server-Sent Events (SSE)**. Otimizada com `System.Threading.Channels` e **Tailwind CSS v4 Native DLL** para performance aprimorada e segurança de nível corporativo.

## 🏗️ Visão Geral da Arquitetura

A solução segue uma estrutura modular projetada para desenvolvimento de alta velocidade e implantação em containers:

* **Apps**:
    * `ALCI.OrderStream.API`: Hub de eventos reativos com **Minimal APIs**.
    * `ALCI.OrderStream.Client`: Frontend **Blazor WASM** com interface de alta precisão.
* **Core**:
    * `ALCI.OrderStream.Domain`: Lógica de negócio compartilhada.
* **Tests**:    
    * `ALCI.OrderStream.Api.Tests`: Testes de **unidade e integração** para o pipeline de eventos utilizando **xUnit**.
    * `ALCI.OrderStream.Client.Tests`: Testes de **componentes de interface (UI)** para Blazor via **bUnit** e **xUnit**.
    * `ALCI.OrderStream.E2E.Tests`: Testes de **ponta a ponta (E2E)** com **Microsoft Playwright** e **xUnit**, validando todo o ciclo de vida do **SSE**, da API ao Navegador.
* **Tools**:
    * `Tailwind`: Compilação de CSS de alta velocidade via binários especializados do **TailwindCSS** (**Windows & Linux**) para melhor performance e execução de nível nativo. Workflow com **zero dependência de Node.js**.
* **Scripts PowerShell**:
    * `start-alci.ps1`: Orquestrador unificado para Desenvolvimento Local (**Windows/HTTPS**).
    * `start-alci-docker.ps1`: Orquestração completa de infraestrutura (**Linux/Docker/HTTP**).

## ⚡ Principais Recursos

* **Resiliência SSE no .NET 10**: Eventos de protocolo customizados para gerenciar conexões do navegador de forma graciosa. Para streaming unidirecional, esta arquitetura supera **WebSockets** e **SignalR** ao utilizar multiplexação nativa do **HTTP/2**, reduzindo overhead e complexidade enquanto mantém atualizações reativas seguras e persistentes.
* **Tailwind v4 Nativo**: UI compilada via binário especializado para feedback instantâneo: **sem npm, sem node_modules**.
* **Multi-Ambiente**: Hierarquia inteligente de `appsettings` que gerencia automaticamente o mapeamento de portas entre Dev Local (**7038/7217**) e Docker (**8080/8081**).
* **Observabilidade**: Logging estruturado e trilha de auditoria (**audit trail**) detalhada.

## 🚀 Execução & Implantação

### Opção A: Desenvolvimento Local (Windows)
Ideal para iterações rápidas de UI/API com **Hot Reload** e **HTTPS**.

```powershell
./start-alci.ps1
```

* **Client**: `https://localhost:7038`
* **API/Swagger**: `https://localhost:7217/swagger`

### Opção B: Infraestrutura Docker (Containerizada)
Ideal para testar o ambiente pronto para produção (**Nginx + API**) em um ecossistema Linux isolado.

```powershell
./start-alci-docker.ps1
```

* **Client**: `http://localhost:8080`
* **API/Swagger**: `http://localhost:8081/swagger`

## 🧪 Executando os Testes

A solução utiliza **xUnit** como padrão unificado para todas as camadas de teste.

### Unidade e Componentes
Para rodar os testes de **API** e **Interface**, execute na raiz da solução:
```powershell
dotnet test
```

### Ponta a Ponta (E2E)
Os testes de **E2E** requerem que a infraestrutura (Windows ou Docker) esteja **ativa**, conforme explicado acima, para validar a comunicação real entre os serviços.

Para rodar os testes do **Playwright** visualizando a automação no navegador:
```powershell
cd tests/ALCI.OrderStream.E2E.Tests
$env:HEADED="1"; dotnet test
```

## 🛠️ Testando a Colisão

1. Abra a URL do **Client** e navegue até a página de rastreamento de um `OrderId` específico.
2. Abra o **Swagger UI** e localize o endpoint **POST** `/orders/{orderId}/simulate`.
3. Execute a simulação.
4. Observe o **Real-Time Stream** via **SSE** atualizando as páginas web instantaneamente.

## 🧹 Protocolo de Encerramento

Ao rodar localmente via `start-alci.ps1`, o script garante uma saída limpa ao encerrar automaticamente todos os processos `dotnet` relacionados, evitando que portas fiquem presas para sua próxima sessão.

---
**André Cirillo**
*(Architect of Light & Chaos | AI Developer & Full-Stack Fixer)*

**Powered by ALCI**
*(Artificial Large Collision Intelligence)*