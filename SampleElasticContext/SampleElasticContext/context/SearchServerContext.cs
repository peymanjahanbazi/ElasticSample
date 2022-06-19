using Elasticsearch.Net;
using Nest;
using SampleElasticContext.entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleElasticContext.context
{
    public class SearchServerContext
    {
        private const string ProductIndexName = "products";
        private const string UriString = "http://localhost:9200/";
        private readonly ElasticClient elasticClient;

        public SearchServerContext()
        {
            var nodes = new Uri[]
              {
                new Uri(UriString),
              };
            var connectionPool = new StaticConnectionPool(nodes);
            var connectionSettings = new ConnectionSettings(connectionPool).DisableDirectStreaming()
                .DefaultMappingFor<Product>(m => m.IndexName(ProductIndexName));
            elasticClient = new ElasticClient(connectionSettings);
            InitializeModels();
        }

        private void InitializeModels()
        {
            var indexDoesNotExist = !elasticClient.Indices.Exists(ProductIndexName).Exists;
            if (indexDoesNotExist)
            {
                Console.WriteLine("Index does not exist");
                var response = elasticClient.Indices.Create(ProductIndexName, index => index.Map<Product>(x => x.AutoMap()));
                Console.WriteLine("Index created. ");
                Console.WriteLine("Server error " + response.ServerError + ", for index " + response.Index);

                Client.IndexDocument<Product>(new Product
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

                Client.IndexDocument<Product>(new Product
                {
                    ProductId = 2,
                    Description = "Description Product 2",
                    ProductName = "Product No 2",
                    Price = 1200,
                    Tags = new List<Tag> {
                        new Tag { Name = "Color", Value = "Yellow" } ,
                        new Tag { Name = "Color", Value = "Blue" },
                        new Tag { Name = "Size", Value = "Small" },
                        new Tag { Name = "Size", Value = "XX-Small" }
                    }
                });
                Console.WriteLine("Two sample documents are inserted");
            }
        }

        public ElasticClient Client
        {
            get { return elasticClient; }
        }
    }
}