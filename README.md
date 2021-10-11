# Ubisoft Feedback Service
This repository is a technical test for Ubisoft.

The project is a micro-service RESTful API made with [.NET Core 5.0](https://docs.microsoft.com/es-es/aspnet/core/?view=aspnetcore-5.0) that will allow users to share feedback on their last game session and 
allow visibility to a live operations team.

Using [Domain Driven Design](https://en.wikipedia.org/wiki/Domain-driven_design) architecture and following [SOLID](https://en.wikipedia.org/wiki/SOLID) principles.

Also using:
- Entity Framework
- Fluent Validation
- Automapping
- Autofac IoC container.

The API can be served using [Docker](https://docs.docker.com/get-started/overview/) containers.

# Clone, Build and Run 
* `git clone https://github.com/blacksmith94/UbisoftConnect.FeedbackService.git`
* `cd UbisoftConnect.FeedbackService`
* `docker-compose build`

- The build pipeline is configured in a way that when running the above build command, firstly it will build and run the [Integration](https://en.wikipedia.org/wiki/Integration_testing) and [Unit](https://en.wikipedia.org/wiki/Unit_testing) tests, if they do not pass, the API won't be built and it will show where did the tests fail.

Serve api:
* `docker-compose up api`

Run integration and unit tests:
* `docker-compose up test`

The API will be served at http://localhost:5001/api

## OpenAPI Specification

The project includes API documentation with [Swagger](https://swagger.io/).

Served at http://localhost:5001/swagger
