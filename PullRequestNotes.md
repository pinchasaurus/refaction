## Introduction

I have refactored this project in the same manner that I would at my workplace.  Indeed, I recently built a comparable Web API service from scratch for several of our existing databases, so I have included some of those goodies here (i.e., NSwag and OWIN).

## Constraints

While refactoring the project, I adhered to these constraints:

* Perform all work in Visual Studio 2015.
* Do not alter the design of the existing database.
* Do not change the version of .NET Framework.

The directions indicate that Visual Studio 2015 is preferred, and the other constraints often exist when working on production systems.

## Changes

I refactored the project as follows:

* Renamed the solution and namespaces to Refaction so they match the name on GitHub (and because refactor-me is not a valid name for a .NET namespace).

* Separated concerns and created layers for Data, Service, Tests, and Common code.

* Added Entity Framework for object-relational mapping, and generated Product and ProductOption entities (and DbContext) from existing database.

* Added repositories to persist/retrieve Product and ProductOption models using an underlying data store.

* Translated existing SQL code into Language Integrated Query (LINQ) for their respective entities.

* Added unit tests for 100% code coverage of controller, repositories, and common code using fake (in-memory) database classes.

* Added Open Web Interface for .NET (OWIN) for integration testing using OWIN test server.

* Added dependency injection using Ninject.

* Added integration tests for end-to-end testing of OWIN test server with in-memory database.

* Added mocking layer using strict mocks to guarantee precise behavior of controller, repositories, and entity framework classes (any unexpected behavior causes test failure).

* Added Cross-Origin Resource Sharing (CORS) capability to the service.

* Added automatic generation of Swagger document using NSwag, and made Swagger UI the default page.

## Comments

If the original constraints were lifted, then I would migrate the project to Visual Studio 2017, enable database migrations, add a logging layer to record controller activity in a database (or other store), and work on upgrading the project to .NET Core and Entity Framework Core.  If performance was a concern, then I would also convert all controller actions to asynchronous methods to free up threads in the application pool.

The ProductsController class is a little large, but all of the Web API routes begin with "/products", so it makes sense to keep everything there.  If the ProductOption routes began with "/productOptions", then it would make sense to move those methods into a different controller.

Even though the models and the entities have the same properties, I implemented a separation of concerns between the domain models and the persistence layer.  A repository's job is to persist/retrieve model objects using the underlying store, and the domain should have no knowledge of how this is done.  This allows domain models to be decoupled from the underlying database structure.

The ADO.Net portions of the code were vulnerable to SQL injection attacks, and they were tightly coupled with the model classes.  I moved this code to the repositories, and translated it to Entity Framework and Language Integrated Query (LINQ).

The original code had no unit tests, so I implemented automated tests using fake (in-memory) database objects.  I was not satisfied with this approach, so I replaced it with strict mocks which decouple my unit tests and guarantee their behavior.

The directions indicate that the Web API should not return the ProductId property of ProductOption objects.  As a quick fix, I added a [JsonIgnore] attribute to this property; however, this is an imperfect solution because it will stop the property from being serialized in other situations.  A better solution would involve Data Transfer Objects or a custom serializer, but these seemed like overkill in this instance.

