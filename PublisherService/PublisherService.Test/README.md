# 🧪 PublisherService.Test

<p align="center">
  <img src="https://img.shields.io/badge/.NET-10.0-512BD4?style=for-the-badge&logo=dotnet" alt=".NET 10" />
  <img src="https://img.shields.io/badge/xUnit-2.7.0-4CAF50?style=for-the-badge&logo=xunit" alt="xUnit" />
  <img src="https://img.shields.io/badge/Moq-4.20-blue?style=for-the-badge" alt="Moq" />
  <img src="https://img.shields.io/badge/FluentAssertions-6.12-orange?style=for-the-badge" alt="FluentAssertions" />
</p>

This project contains the automated unit test suite for the **PublisherService** microservice. It ensures the reliability, correctness, and stability of both the core business logic and the GraphQL API layer.

---

## 🏗️ Architecture & Isolation Strategy

To ensure our unit tests are fast and reliable, we use strict isolation. We test one layer at a time, mocking the dependencies immediately below it so we do not rely on live databases or external systems.

```mermaid
graph TD
    subgraph PublisherService.Test [PublisherService.Test]
        T1[PublisherQueryTests]:::testClass
        T2[PublisherServiceTests]:::testClass
    end

    subgraph PublisherService.Api [API Layer]
        Q[PublisherQuery<br/>GraphQL Resolver]:::apiClass
    end

    subgraph PublisherService.Application [Application Layer]
        S[PublisherService<br/>Business Logic]:::appClass
    end

    subgraph PublisherService.Infrastructure [Infrastructure Layer]
        R[(PublisherRepository<br/>EF Core)]:::dbClass
    end

    %% API Tests Flow
    T1 -->|Tests| Q
    T1 -.->|Mocks| S
    
    %% Application Tests Flow
    T2 -->|Tests| S
    T2 -.->|Mocks| R

    %% Normal execution flow
    Q ===>|Uses| S
    S ===>|Uses| R

    classDef testClass fill:#e1f5fe,stroke:#01579b,stroke-width:2px,color:#000;
    classDef apiClass fill:#fff3e0,stroke:#e65100,stroke-width:2px,color:#000;
    classDef appClass fill:#e8f5e9,stroke:#1b5e20,stroke-width:2px,color:#000;
    classDef dbClass fill:#fce4ec,stroke:#880e4f,stroke-width:2px,color:#000;
```

---

## 📂 Project Structure & Test Coverage

We employ a single test project structured into folders that precisely mirror the service layers. 

### 1. Application Layer (`PublisherServiceTests.cs`)
Validates core business rules, input validation, and Entity-to-DTO mapping.

| Test Case Name | Scenario Tested | Expected Outcome |
| :--- | :--- | :--- |
| `GetPublisherByIdAsync_WithValidId` | Valid positive ID | Returns mapped `PublisherDto` |
| `GetPublisherByIdAsync_WithInvalidId` | ID is `<= 0` | Returns `null` immediately (no DB call) |
| `GetPublisherByIdAsync_WhenPublisherNotFound` | ID does not exist in DB | Returns `null` |
| `GetPublishersByNameAsync_WithEmptyName` | Search string is empty | Returns empty list (no DB call) |
| `GetPublishersByNameAsync_WithValidName` | Valid search string | Returns mapped list of DTOs |

### 2. API Layer (`PublisherQueryTests.cs`)
Validates the GraphQL entry points, request delegation, and expected error handling formatting.

| Test Case Name | Scenario Tested | Expected Outcome |
| :--- | :--- | :--- |
| `GetPublisherByIdAsync_WhenPublisherExists` | Resolver finds publisher | Returns `PublisherDto` |
| `GetPublisherByIdAsync_WhenPublisherDoesNotExist` | Resolver cannot find publisher | Throws controlled `GraphQLException` |
| `GetPublishersByName_CallsServiceAndReturnsList` | Resolver performs search | Returns list without modification |

---

## 🚀 Execution Guide

### Visual Studio
The easiest way to run the test suite is visually:
1. Press `Ctrl + E, T` to open the **Test Explorer**.
2. Click the green ▶️ **Run All** button in the top left.

### .NET CLI
For CI/CD pipelines or rapid terminal usage, run this from the repository root:

```bash
dotnet test PublisherService/PublisherService.Test/PublisherService.Test.csproj --verbosity normal
```
