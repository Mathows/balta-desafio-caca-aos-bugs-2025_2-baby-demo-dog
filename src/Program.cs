using BugStore.Handlers.Customers;
using BugStore.Handlers.Products;
using BugStore.Handlers.Orders;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<BugStore.Data.AppDbContext>();
builder.Services.AddTransient<BugStore.Handlers.Customers.Handler>();
builder.Services.AddTransient<BugStore.Handlers.Products.Handler>();
builder.Services.AddTransient<BugStore.Handlers.Orders.Handler>();

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.MapPost("/v1/customers", (BugStore.Requests.Customers.Create request, BugStore.Handlers.Customers.Handler handler) =>
{
    var response = handler.Handle(request);
    return Results.Created($"/v1/customers/{response.Id}", response);
});

app.MapGet("/v1/customers", (BugStore.Handlers.Customers.Handler handler) =>
{
    var request = new BugStore.Requests.Customers.Get();
    var response = handler.Handle(request);
    return Results.Ok(response);
});

app.MapGet("/v1/customers/{id:guid}", (Guid id, BugStore.Handlers.Customers.Handler handler) =>
{
    var request = new BugStore.Requests.Customers.GetById { Id = id };
    var response = handler.Handle(request);

    if (response.Customer == null)
        return Results.NotFound($"Customer with ID {id} not found.");

    return Results.Ok(response);
});

app.MapPut("/v1/customers/{id:guid}", (Guid id, BugStore.Requests.Customers.Update requestBody, BugStore.Handlers.Customers.Handler handler) =>
{
    if (id != requestBody.Id)
        return Results.BadRequest("ID mismatch between URL and request body.");

    var response = handler.Handle(requestBody);

    if (response.Customer == null)
        return Results.NotFound($"Customer with ID {id} not found.");

    return Results.Ok(response);
});

app.MapDelete("/v1/customers/{id:guid}", (Guid id, BugStore.Handlers.Customers.Handler handler) =>
{
    var request = new BugStore.Requests.Customers.Delete { Id = id };
    var response = handler.Handle(request);

    if (!response.Success)
        return Results.NotFound($"Customer with ID {id} was not deleted (possibly not found).");

    return Results.NoContent();
});

app.MapGet("/v1/products", (BugStore.Handlers.Products.Handler handler) =>
{
    var request = new BugStore.Requests.Products.Get();
    var response = handler.Handle(request);
    return Results.Ok(response);
});

app.MapGet("/v1/products/{id:guid}", (Guid id, BugStore.Handlers.Products.Handler handler) =>
{
    var request = new BugStore.Requests.Products.GetById { Id = id };
    var response = handler.Handle(request);

    if (response.Product == null)
        return Results.NotFound($"Product with ID {id} not found.");

    return Results.Ok(response);
});

app.MapPost("/v1/products", (BugStore.Requests.Products.Create request, BugStore.Handlers.Products.Handler handler) =>
{
    var response = handler.Handle(request);
    return Results.Created($"/v1/products/{response.Id}", response);
});

app.MapPut("/v1/products/{id:guid}", (Guid id, BugStore.Requests.Products.Update requestBody, BugStore.Handlers.Products.Handler handler) =>
{
    if (id != requestBody.Id)
        return Results.BadRequest("ID mismatch between URL and request body.");

    var response = handler.Handle(requestBody);

    if (response.Product == null)
        return Results.NotFound($"Product with ID {id} not found.");

    return Results.Ok(response);
});

app.MapDelete("/v1/products/{id:guid}", (Guid id, BugStore.Handlers.Products.Handler handler) =>
{
    var request = new BugStore.Requests.Products.Delete { Id = id };
    var response = handler.Handle(request);

    if (!response.Success)
        return Results.NotFound($"Product with ID {id} was not deleted (possibly not found).");

    return Results.NoContent();
});

app.MapGet("/v1/orders/{id:guid}", (Guid id, BugStore.Handlers.Orders.Handler handler) =>
{
    var request = new BugStore.Requests.Orders.GetById { Id = id };
    var response = handler.Handle(request);

    if (response.Order == null)
        return Results.NotFound($"Order with ID {id} not found.");

    return Results.Ok(response);
});

app.MapPost("/v1/orders", (BugStore.Requests.Orders.Create request, BugStore.Handlers.Orders.Handler handler) =>
{
    var response = handler.Handle(request);
    return Results.Created($"/v1/orders/{response.Id}", response);
});

app.Run();