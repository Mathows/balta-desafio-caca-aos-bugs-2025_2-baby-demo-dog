using BugStore.Requests.Customers;
using BugStore.Responses.Customers;
using BugStore.Handlers.Customers;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<BugStore.Data.AppDbContext>();
builder.Services.AddTransient<BugStore.Handlers.Customers.Handler>();

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.MapPost("/v1/customers", (BugStore.Requests.Customers.Create request, Handler handler) =>
{
    var response = handler.Handle(request);


    return Results.Created($"/v1/customers/{response.Id}", response);
});


app.MapGet("/v1/customers", (Handler handler) =>
{
    var request = new BugStore.Requests.Customers.Get();
    var response = handler.Handle(request);

    return Results.Ok(response);
});

app.MapGet("/v1/customers/{id:guid}", (Guid id, Handler handler) =>
{
    var request = new BugStore.Requests.Customers.GetById { Id = id };
    var response = handler.Handle(request);

    if (response.Customer == null)
    {
        return Results.NotFound($"Customer with ID {id} not found.");
    }

    return Results.Ok(response);
});


app.MapPut("/v1/customers/{id:guid}", (Guid id, BugStore.Requests.Customers.Update requestBody, Handler handler) =>
{
    if (id != requestBody.Id)
    {
        return Results.BadRequest("ID mismatch between URL and request body.");
    }

    var response = handler.Handle(requestBody);

    if (response.Customer == null)
    {
        return Results.NotFound($"Customer with ID {id} not found.");
    }

    return Results.Ok(response);
});

app.MapDelete("/v1/customers/{id:guid}", (Guid id, Handler handler) =>
{
    var request = new BugStore.Requests.Customers.Delete { Id = id };
    var response = handler.Handle(request);

    if (!response.Success)
    {
        return Results.NotFound($"Customer with ID {id} was not deleted (possibly not found).");
    }

    return Results.NoContent();
});

app.MapGet("/v1/products", () => "Hello World!");
app.MapGet("/v1/products/{id}", () => "Hello World!");
app.MapPost("/v1/products", () => "Hello World!");
app.MapPut("/v1/products/{id}", () => "Hello World!");
app.MapDelete("/v1/products/{id}", () => "Hello World!");

app.MapGet("/v1/orders/{id}", () => "Hello World!");
app.MapPost("/v1/orders", () => "Hello World!");

app.Run();