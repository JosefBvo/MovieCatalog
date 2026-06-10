# Movie Catalog

## Overview

Movie Catalog is a C# console application that allows users to manage and explore a collection of movies stored in a SQLite database. Users can add and remove movies, manage basic user information, submit movie ratings, search for movies in the DB, and view basic information such as name, genre, and rating.
While the application itself is intentionally simple, this repository serves as a demonstration of a secure CI/CD pipeline using GitHub Actions and modern DevSecOps practices.

## Features

### Movie Management

* Add movies to the catalog
* Delete movies from the catalog
* View all movies
* Search for movies by title
* Filter movies by genre

### User Management

* Create users
* Update usernames
* Delete users
* View registered users

### Ratings

* Add ratings to movies
* View average movie ratings
* View top-rated movies

### Database

* SQLite database backend
* Automatic database initialization and seeding
* Preloaded genres, movies, users, ratings, and watchlist data

---

## Project Structure

```text
MovieCatalog/
├── .github/
│   └── workflows/
├── MovieCatalog/
│   ├── Program.cs
│   ├── MovieService.cs
│   ├── DatabaseHelper.cs
│   └── ...
├── MovieCatalog.Tests/
└── README.md
```

### Key Components

#### Program.cs

Handles the console interface and menu navigation.

#### MovieService.cs

Contains the application's business logic, including movie management, user management, ratings, and database queries.

#### DatabaseHelper.cs

Responsible for database initialization, schema creation, seeding, and connection management.

#### MovieCatalog.Tests

Contains automated unit tests executed during the CI pipeline.

---

## Running the Application

### Prerequisites

* .NET 10 SDK or later
* IDE (I personally used Visual Studio Community 2026)

### Docker 

```
docker build -t moviecatalog .

docker run --rm -it moviecatalog
```

### Build Locally

```
dotnet build
```

#### Run

```
dotnet run --project MovieCatalog
```

The application will automatically create and initialize the SQLite database if it does not already exist.

---

## CI/CD Pipeline

This repository uses GitHub Actions to automate building, testing, and security analysis.

### Continuous Integration

On every push and pull request:

1. Restore dependencies
2. Build the solution
3. Execute unit tests
4. Verify the application compiles successfully

### Continuous Delivery

Once a PR is merged into default branch:

1. The app is containerized into a docker image
2. The image is pushed to GitHub Container Registry (GHCR) for easy deployment

### Security Controls

#### CodeQL Analysis

Weekly CodeQL scans perform static application security testing to identify potential security vulnerabilities and coding issues.

#### Dependabot

Dependabot continuously monitors project dependencies and automatically creates pull requests when security updates or dependency upgrades become available.

#### Automated Testing

Unit tests help validate core application functionality and reduce the risk of regressions during future development.

#### Container Security

Builds to ensure that the final production image contains only the necessary runtime files, reducing attack surface.

---

## Purpose of This Repository

This repository (as of 6/8/2026) serves as a functional showcase of modern DevOps engineering, demonstrating how to bridge the gap between local C# development and automated cloud delivery using Docker and GitHub Actions.
