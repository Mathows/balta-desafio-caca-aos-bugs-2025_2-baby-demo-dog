using BugStore.Models;

namespace BugStore.Requests.Orders;

public class Update
{
    public Guid Id { get; set; }
    public List<OrderLine> Lines { get; set; } = new();
}
