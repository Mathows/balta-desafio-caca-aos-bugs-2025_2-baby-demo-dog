using BugStore.Models;
using System.Collections.Generic;

namespace BugStore.Responses.Orders;

public class Get
{
 public List<Order> Orders { get; set; } = new List<Order>();
 public int TotalCount { get; set; }
}
