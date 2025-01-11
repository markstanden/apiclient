# API Client

A simple API client designed to practice building a console application in C#.

## Goals

### Primary Goals
- Write all tests first, following TDD principles properly
- Learn to structure C# projects in a clean, sensible way
- Get to grips with C# design patterns, particularly the builder pattern
- Set up a robust CI/CD pipeline to ensure:
    - [x] Clean builds
    - [x] Passing tests
    - [x] Good code coverage (>=80%)
    - [x] Consistent formatting

### Secondary Goals
- Get the hang of C# naming conventions
- Learn to write clean tests using NSubstitute
- Master proper argument validation patterns in C#

## Development Setup

### Prerequisites
- .NET 8.0
- CSharpier (Global Tool): `dotnet tool install -g csharpier`

### Git Hooks
This project uses Git hooks for automated code formatting. To set up:

1. Ensure CSharpier is installed (see prerequisites)
2. Configure Git to use the project hooks:
   ```bash
   git config core.hooksPath .hooks
   ```

The pre-commit hook will automatically format C# files using CSharpier before each commit.

### Code Style
- Code formatting is handled automatically by CSharpier
- Configuration can be found in `.csharpierrc`

## CI/CD Pipeline
The project uses GitHub Actions workflows to enforce code quality:

### Workflow Structure
- **Code Quality**: Validates code formatting using CSharpier
- **Compilation**: Ensures the solution builds successfully
- **Code Coverage**: Runs tests and verifies 80% minimum test coverage

### Key Features
- Optimized with caching for faster builds
- Automated checks on PRs to main branch
- Workflows work independently
- Manual triggers for development testing
- Failed checks block PR merging

## Project Structure & Patterns

### Builder Pattern
The project implements the Builder pattern for configuration, allowing flexible and readable setup:
```csharp
var config = new ApiClientConfigurationBuilder()
    .WithBaseUrl("https://api.example.com")
    .WithBearerToken("secret")
    .Build();
```

### Testing Approach
- Test-Driven Development (TDD) methodology
- Using xUnit as the test framework
- NSubstitute for mocking, particularly useful for HTTP client testing
- Tests follow Arrange-Act-Assert pattern

### Project Organization
- Core API client functionality in `ApiClient` project
- Unit tests in `ApiClient.Tests.Unit`
- Clear separation of configuration and services
- Interface-based design for better testability