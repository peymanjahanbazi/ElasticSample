using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleElasticContext.entities;

internal class Product
{
    public int ProductId { get; set; }
    public string ProductName { get; set; }
    public decimal Price { get; set; }
    public string Description { get; set; }
    public List<Tag> Tags { get; set; }

    public override string? ToString()
    {
        return $"id = {ProductId}, name = {ProductName}, Tags(Count) = {Tags.Count}";
    }
}