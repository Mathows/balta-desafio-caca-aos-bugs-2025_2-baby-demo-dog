using BugStore.Models;

namespace BugStore.Responses.Customers;

public class Get
{
    public List<Customer> Customers { get; set; } = new List<Customer>();
    public int TotalCount { get; set; }

}