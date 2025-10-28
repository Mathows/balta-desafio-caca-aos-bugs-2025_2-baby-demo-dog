using System;
using System.Collections.Generic;
using System.Linq;
using BugStore.Models;

namespace BugStore.Handlers.Orders;

public class Handler
{
 private static readonly List<Order> _orders = new();
 private static readonly List<Product> _products = new()
 {
 new Product
 {
 Id = Guid.Parse("B0C1D2E3-0000-0000-0000-000000000001"),
 Title = "BugStore Mug",
 Description = "A cool mug for bug hunting.",
 Slug = "mug-bugstore",
 Price =15.00m
 }
 };

 // CREATE
 public Order Handle(Requests.Orders.Create request)
 {
 var order = new Order
 {
 Id = Guid.NewGuid(),
 CustomerId = request.CustomerId,
 CreatedAt = DateTime.UtcNow,
 UpdatedAt = DateTime.UtcNow,
 Lines = new List<OrderLine>()
 };

 // Build lines with product references
 foreach (var line in request.Lines)
 {
 var product = _products.FirstOrDefault(p => p.Id == line.ProductId);

 var orderLine = new OrderLine
 {
 Id = Guid.NewGuid(),
 OrderId = order.Id,
 ProductId = line.ProductId,
 Quantity = line.Quantity,
 Product = product,
 Total = product != null ? product.Price * line.Quantity :0
 };

 order.Lines.Add(orderLine);
 }

 _orders.Add(order);
 return order;
 }

 // GET ALL
 public Responses.Orders.Get Handle(Requests.Orders.Get request)
 {
 var filtered = _orders.AsEnumerable();

 if (request.CustomerId.HasValue)
 filtered = filtered.Where(o => o.CustomerId == request.CustomerId.Value);

 return new Responses.Orders.Get
 {
 Orders = filtered.ToList(),
 TotalCount = filtered.Count()
 };
 }

 // GET BY ID
 public Responses.Orders.GetById Handle(Requests.Orders.GetById request)
 {
 var found = _orders.FirstOrDefault(o => o.Id == request.Id);

 return new Responses.Orders.GetById
 {
 Order = found
 };
 }

 // UPDATE
 public Responses.Orders.Update Handle(Requests.Orders.Update request)
 {
 var existing = _orders.FirstOrDefault(o => o.Id == request.Id);

 if (existing != null)
 {
 existing.Lines = request.Lines;
 existing.UpdatedAt = DateTime.UtcNow;
 }

 return new Responses.Orders.Update
 {
 Order = existing
 };
 }

 // DELETE
 public Responses.Orders.Delete Handle(Requests.Orders.Delete request)
 {
 var before = _orders.Count;
 _orders.RemoveAll(o => o.Id == request.Id);
 var deleted = _orders.Count < before;

 return new Responses.Orders.Delete
 {
 Success = deleted
 };
 }
}
