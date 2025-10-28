using BugStore.Models;

namespace BugStore.Handlers.Customers;

public class Handler
{
    private static readonly List<Customer> _customers = new List<Customer>
    {
        new Customer
        {
            Id = Guid.Parse("A1B2C3D4-0000-0000-0000-000000000001"),
            Name = "Alice",
            Email = "alice@bugstore.com",
            Phone = "1111-1111",
            BirthDate = DateTime.Parse("1990-01-01")
        }
    };

    // CREATE
    public Customer Handle(Requests.Customers.Create request)
    {
        var customer = new Customer
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Email = request.Email,
            Phone = request.Phone,
            BirthDate = request.BirthDate
        };

        _customers.Add(customer);
        return customer;
    }

    // GET ALL (LIST)
    public Responses.Customers.Get Handle(Requests.Customers.Get request)
    {
        return new Responses.Customers.Get
        {
            Customers = _customers.ToList(),
            TotalCount = _customers.Count
        };
    }

    // GET BY ID
    public Responses.Customers.GetById Handle(Requests.Customers.GetById request)
    {
        var foundCustomer = _customers.FirstOrDefault(c => c.Id == request.Id);

        return new Responses.Customers.GetById
        {
            Customer = foundCustomer
        };
    }

    // UPDATE
    public Responses.Customers.Update Handle(Requests.Customers.Update request)
    {
        var customerToUpdate = _customers.FirstOrDefault(c => c.Id == request.Id);

        if (customerToUpdate != null)
        {
            customerToUpdate.Name = request.Name;
            customerToUpdate.Email = request.Email;
            customerToUpdate.Phone = request.Phone;
            customerToUpdate.BirthDate = request.BirthDate;
        }

        return new Responses.Customers.Update
        {
            Customer = customerToUpdate
        };
    }

    // DELETE
    public Responses.Customers.Delete Handle(Requests.Customers.Delete request)
    {
        var initialCount = _customers.Count;
        _customers.RemoveAll(c => c.Id == request.Id);
        var deleted = _customers.Count < initialCount;

        return new Responses.Customers.Delete
        {
            Success = deleted
        };
    }

}