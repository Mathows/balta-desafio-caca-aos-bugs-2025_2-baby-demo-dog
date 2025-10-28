using BugStore.Models;
using System.Collections.Generic;

namespace BugStore.Responses.Products;

public class Get
{
 public List<Product> Products { get; set; } = new List<Product>();
 public int TotalCount { get; set; }
}