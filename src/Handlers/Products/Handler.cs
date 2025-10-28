using BugStore.Models;

namespace BugStore.Handlers.Products;

public class Handler
{
    private static readonly List<Product> _products = new List<Product>
    {
        new Product
        {
            Id = Guid.Parse("B0C1D2E3-0000-0000-0000-000000000001"),
            Title = "BugStore Mug",
            Description = "A cool mug for bug hunting.",
            Slug = "mug-bugstore",
            Price = 15.00m
        }
    };

    // CREATE
    public Product Handle(Requests.Products.Create request)
    {
        var product = new Product
        {
            Id = Guid.NewGuid(),
            Price = request.Price,
            Description = request.Description,
            Slug = request.Slug,
            Title = request.Title
        };

        _products.Add(product);
        return product;
    }

    // GET ALL (LIST)
    public Responses.Products.Get Handle(Requests.Products.Get request)
    {
        return new Responses.Products.Get
        {
            Products = _products.ToList(),
            TotalCount = _products.Count
        };
    }

    // GET BY ID
    public Responses.Products.GetById Handle(Requests.Products.GetById request)
    {
        var foundProduct = _products.FirstOrDefault(p => p.Id == request.Id);

        return new Responses.Products.GetById
        {
            Product = foundProduct
        };
    }

    // UPDATE
    public Responses.Products.Update Handle(Requests.Products.Update request)
    {
        var productToUpdate = _products.FirstOrDefault(p => p.Id == request.Id);

        if (productToUpdate != null)
        {
            productToUpdate.Title = request.Title;
            productToUpdate.Description = request.Description;
            productToUpdate.Slug = request.Slug;
            productToUpdate.Price = request.Price;
        }

        return new Responses.Products.Update
        {
            Product = productToUpdate
        };
    }

    // DELETE
    public Responses.Products.Delete Handle(Requests.Products.Delete request)
    {
        var initialCount = _products.Count;
        _products.RemoveAll(p => p.Id == request.Id);
        var deleted = _products.Count < initialCount;

        return new Responses.Products.Delete
        {
            Success = deleted
        };
    }
}
