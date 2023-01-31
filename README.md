# Lab 01: Setting up .NET 7.0 Minimal API

```
dotnet new webapi -minimal -f net7.0 -n HelloApi
```

- Add Docker files

```
make up_build_debug

curl -X GET http://localhost:5000/weatherforecast
```

- See [Swagger UI](http://localhost:5000/swagger/index.html)

# Lab 02: API Quick Reference

[Minimal APIs quick reference](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/minimal-apis?view=aspnetcore-7.0)

- Dependency Injection
- HttpClient & IHttpClientFactory
- Routing
