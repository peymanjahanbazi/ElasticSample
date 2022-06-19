using SampleElasticContext.context;
using SampleElasticContext.entities;
using SampleElasticContext.repositories;

SearchServerContext context = new();
ElasticRepository repository = new(context);

Console.WriteLine("Sample document insert");
repository.Insert(new Product
{
    ProductId = 1,
    Description = "Description Product 1",
    ProductName = "Product No 1",
    Price = 1200,
    Tags = new List<Tag> {
        new Tag { Name = "Color", Value = "Red" } ,
        new Tag { Name = "Color", Value = "Blue" },
        new Tag { Name = "Size", Value = "Large" },
        new Tag { Name = "Size", Value = "XX-Large" }
    }
});

var result = repository.SearchById(1);
result.ForEach(p => Console.WriteLine(p));

Console.WriteLine("Program executed successfuly");