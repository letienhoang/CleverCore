# TeduCoreApp

Tedu Core App for Course TEDU-17

## Introduction

This is a forked version of the original [CleverCore](https://github.com/teduinternational/CleverCore) project, used as a foundation for the lessons in the TEDU-17 course.

In this version, I have added automated unit testing using **xUnit** to improve code reliability and quality.

## Updates and Features

* Integrated **xUnit** framework for writing unit tests.
* Added multiple test projects:
  * `CleverCore.Application.Dapper.Test`
  * `CleverCore.Application.Test`
  * `CleverCore.Infrastructure.Test`
  * `CleverCore.Data.EF.Test`
  * `CleverCore.Utilities.Test`
  * `CleverCore.WebApp.Test`
  * `CleverCore.WebApi.Test`
* Each test project contains unit tests for its corresponding application layer..
* Pre-configured for running tests via Visual Studio or the `dotnet test` command line.

## How to Run Unit Tests

1. Open the solution using Visual Studio or VS Code.
2. Restore packages using:

   ```sh
   dotnet restore
   ```
3. Run all test projects:

   ```sh
   dotnet test
   ```

   Or run a specific test project:

   ```sh
   dotnet test CleverCore.Application.Dapper.Test
   ```

## Additional Information

* Original project: [CleverCore](https://github.com/teduinternational/CleverCore)
* TEDU-17 Course: [TEDU-17](https://tedu.com.vn/khoa-hoc/ky-thuat-unit-test-cho-net-developer-29.html)

---

*Updated by [letienhoang](https://github.com/letienhoang) — July 2025*
