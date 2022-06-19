using Elasticsearch.Net;
using Nest;
using SampleElasticContext.context;
using SampleElasticContext.entities;

namespace SampleElasticContext.repositories;

internal class ElasticRepository
{
    private readonly Nest.ElasticClient elastic;

    public ElasticRepository(SearchServerContext client)
    {
        this.elastic = client.Client;
    }

    public List<Product> Search()
    {
        ISearchResponse<Product> searchResponse = elastic.Search<Product>(p => p.Source());
        return searchResponse.Hits.Select(x => x.Source).ToList();
    }

    public void Insert(Product p)
    {
        elastic.IndexDocument<Product>(p);
    }

    internal List<Product> SearchById(int id)
    {
        ISearchResponse<Product>? p = elastic.Search<Product>(
            sd =>
            sd.From(0)
            .Size(10)
            .Query(q => q.Term(p => p.ProductId, id))
        );
        return p.Hits.Select(x => x.Source).ToList();
    }

    internal List<Product> SearchSample1()
    {
        ISearchResponse<Product>? p = elastic.Search<Product>(
            sd =>
            sd.From(0)
            .Size(10)
            .Query(q =>
                q.Term(p => p.ProductId, 1) ||
                q.Terms(p => p.Field(f => f.ProductId)) ||
                q.Bool(b => b.MinimumShouldMatch(1)
                .Should(p => p.Term(p => p.ProductId, 1))

                )
            )
        );
        return p.Hits.Select(x => x.Source).ToList();
    }

    internal SearchResponse<Product> SearchSampleLowlevel()
    {
        SearchResponse<Product>? p = elastic.LowLevel.Search<SearchResponse<Product>>(PostData.Serializable(new
        {
            from = 0,
            size = 15,
            query = new
            {
                Bool = new
                {
                    MinimumShouldMatch = 1
                }
            }
        }));

        return p;
    }
}