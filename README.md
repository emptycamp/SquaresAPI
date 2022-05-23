# Squares API

### Installation guide

1. Clone project.
2. Build project using *Build->Build Solution* in **Visual Studio** or type ```dotnet build``` in console.
3. Update database using ```Update-Database``` in **Visual Studio Package Manager Console** or type ```dotnet ef database update``` in console.
4. Start server using *Debug->Start Without Debugging* in **Visual Studio**

# Implemented features

### Functional

- User can import a list of points
- User can add a point to an existing list
- User can delete a point from an existing list
- User can retrieve the squares identified

### Non-fuctional

* Included prerequisites and steps to launch in `README`
* Added solution code to the `git` repository
* The API is implemented using latest stable .NET Core framework
* The API has persistance layer - Repositories
* Requests do not take more than 5 seconds

### Bonus points stuff!

* Generated documentation tests
* Wrote integration tests (not fully due to time limitation)
* Docker containerization/deployment (not fully due to time limitation)
* Wrote comments/thoughts on more difficult functions


# What I could improve
* Archieve higher test coverage.
* Seperate models and DTO's, map them using [automapper](https://docs.automapper.org/en/stable/Getting-started.html).
* Use generic repositories and extract them into seperate .NET project.
* Dockerize ASP.NET project and MSSQL, setup docker-compose.yml.
