# SoftUni Back-End Technologies Basics (QA, C#)

[![CI](https://github.com/chavdar12/SoftUni-Back-End-Technologies-Basics/actions/workflows/ci.yml/badge.svg)](https://github.com/chavdar12/SoftUni-Back-End-Technologies-Basics/actions/workflows/ci.yml)

Labs and exercises for SoftUni's **Back-End Technologies Basics** course (QA Engineering track). Every program has unit or integration tests, and GitHub Actions builds and tests everything on each push.

## Requirements

- .NET SDK 10
- Node.js 24 (for the JavaScript lessons)
- Docker (optional, lesson 021)

## Running

```bash
dotnet test SoftUni-Back-End-Technologies-Basics.slnx   # all C# tests
npm ci && npm test                                     # all JavaScript tests (Mocha + Chai)
```

Run a single program with `dotnet run --project <path-to-csproj>`, e.g.
`dotnet run --project 004-Exercises-Data-Formats/DataFormats`.

## Lessons

| # | Lesson | What's inside |
|---|--------|---------------|
| 002 | Back-End Technologies Overview | Lecture only |
| 003 | Data Formats: JSON, YAML, XML | `products.json`, `cities.xml`, `countries.yaml` + tests that parse them |
| 004 | Exercise: Data Formats | JSON parser, XML → JSON and YAML → HTML converters |
| 005 | JavaScript Basics | 10 lab tasks |
| 006 | Exercise: JavaScript Basics | 12 tasks |
| 007 | JavaScript Fundamentals | 13 lab tasks |
| 008 | Exercise: JavaScript Fundamentals | 17 tasks |
| 009 | Unit Testing with JavaScript | `sum`, `isSymmetric`, `rgbToHexColor`, `createCalculator` |
| 010 | Exercise: Unit Testing with JavaScript | `analyzeArray`, `isOddOrEven`, `lookupChar`, `mathEnforcer` |
| 011 | Databases Overview | SQL schema, seed and queries, tested against SQLite |
| 012 | Integration Testing | Towns application (EF Core) |
| 013 | Exercise: Integration Testing, part 1 | Libro book manager |
| 014 | Exercise: Integration Testing, part 2 | Movies library |
| 015 | Unit Testing with Mocking | Greeting provider and item service, mocked with Moq |
| 016 | RESTful API Back-End Apps | Homies events Web API + service tests |
| 017 | Exercise: Testing API Controllers | HTTP-level tests for the Homies API (`WebApplicationFactory`) |
| 018 | Code Coverage | Library management and `MyArray` |
| 019 | Mutation Testing | `ArraySearch` and calculator, with tests written to kill mutants |
| 020 | Exam Preparation 1 | Musicians JSON parser, Cinema (JS), Contacts integration tests |
| 021 | Containers: Docker & Docker Compose | Notes minimal API with `Dockerfile` and `compose.yaml` |
| 022 | Entity Framework Introduction | Person register with a one-to-many relationship |
| 023 | ASP.NET Core Introduction | MVC products demo with Razor views |
| 024 | Exam Preparation 2 | Astronauts JSON parser, Shop (JS) |

## Tools per lesson

- **Code coverage (018):** `dotnet test --collect "XPlat Code Coverage"` writes Cobertura reports to `TestResults/`. CI uploads them as an artifact.
- **Mutation testing (019):** `dotnet tool install -g dotnet-stryker`, then run `dotnet stryker` inside `019-Mutation-Testing/ArrayTools.Tests` or `Calculator.Tests`.
- **Docker (021):** `docker compose -f 021-Containers-Docker-and-Docker-Compose/compose.yaml up --build`, then open <http://localhost:8080/api/notes>.

## Differences from the course skeletons

- **SQLite instead of SQL Server / MongoDB.** Integration tests run against in-memory SQLite, so they need no database server locally or in CI. Running a console app creates a local `*.db` file (git-ignored).
- **JavaScript tasks return their output** instead of calling `console.log`, so they can be unit tested. Multi-line output is returned as an array of lines.
- **Homies (016/017) is a JSON Web API** rather than an MVC app with ASP.NET Identity. The caller passes its user id in an `X-User-Id` header.
- **Bugs fixed from the reference solutions:**
  - `MyArray.Replace` accepted `position == Length` (off-by-one).
  - The movies library didn't await deletes.
  - Book and movie year limits were hard-coded to 2024/2030.
  - `ItemService` never validated item names.
  - `TownController.UpdateTown` accepted non-positive populations.
  - The duplicate-ISBN/title/ULID checks were missing.
  - Odd Occurrences reordered numeric words.
  - String Substring was case-sensitive.
  - Cinema sold `undefined` tickets.
  - Shop dropped multi-word product names.
  - The MVC demo formatted prices with the server's region settings.

## Sources

The task list and specifications come from SoftUni course materials and from public student repositories, including [AlexxanderD/Back-End-Technologies-Basics](https://github.com/AlexxanderD/Back-End-Technologies-Basics) and [SimeonSavov/Back-End-Basics](https://github.com/SimeonSavov/Back-End-Basics). Those repositories have no license, so the code here is a fresh implementation, not a copy.
